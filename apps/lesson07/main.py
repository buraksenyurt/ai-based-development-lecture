"""
Document Embedding Tool
Reads documents from a source path, converts them to vectors using LM Studio
text embedding, and stores them in Qdrant vector database.
"""

import argparse
import os
import sys
import time
import uuid
from pathlib import Path

from openai import OpenAI
from qdrant_client import QdrantClient
from qdrant_client.models import Distance, PointStruct, VectorParams
from tqdm import tqdm

# ── Configuration ────────────────────────────────────────────────────────────

LM_STUDIO_BASE_URL = "http://127.0.0.1:1234/v1"
EMBEDDING_MODEL = "text-embedding-embeddinggemma-300m"
EMBEDDING_DIM = 768          # embeddinggemma-300m output dimension

QDRANT_HOST = "localhost"
QDRANT_PORT = 6333
QDRANT_COLLECTION = "documents"

CHUNK_SIZE = 500             # characters per chunk
CHUNK_OVERLAP = 50           # overlap between consecutive chunks

SUPPORTED_EXTENSIONS = {".txt", ".md", ".pdf", ".docx"}

# ── Clients ──────────────────────────────────────────────────────────────────

def build_clients():
    lm = OpenAI(base_url=LM_STUDIO_BASE_URL, api_key="lm-studio")
    qdrant = QdrantClient(host=QDRANT_HOST, port=QDRANT_PORT)
    return lm, qdrant


# ── Document loading ──────────────────────────────────────────────────────────

def load_text_file(path: Path) -> str:
    return path.read_text(encoding="utf-8", errors="replace")


def load_pdf(path: Path) -> str:
    from pypdf import PdfReader
    reader = PdfReader(str(path))
    return "\n".join(page.extract_text() or "" for page in reader.pages)


def load_docx(path: Path) -> str:
    from docx import Document
    doc = Document(str(path))
    return "\n".join(p.text for p in doc.paragraphs)


def load_document(path: Path) -> str:
    ext = path.suffix.lower()
    if ext == ".pdf":
        return load_pdf(path)
    if ext == ".docx":
        return load_docx(path)
    return load_text_file(path)


def collect_files(source: Path) -> list[Path]:
    if source.is_file():
        return [source] if source.suffix.lower() in SUPPORTED_EXTENSIONS else []
    files = []
    for f in source.rglob("*"):
        if f.is_file() and f.suffix.lower() in SUPPORTED_EXTENSIONS:
            files.append(f)
    return sorted(files)


# ── Chunking ─────────────────────────────────────────────────────────────────

def chunk_text(text: str) -> list[str]:
    text = text.strip()
    if not text:
        return []
    chunks = []
    start = 0
    while start < len(text):
        end = start + CHUNK_SIZE
        chunks.append(text[start:end])
        start += CHUNK_SIZE - CHUNK_OVERLAP
    return chunks


# ── Embedding ─────────────────────────────────────────────────────────────────

def embed_texts(client: OpenAI, texts: list[str]) -> list[list[float]]:
    response = client.embeddings.create(model=EMBEDDING_MODEL, input=texts)
    return [item.embedding for item in response.data]


# ── Qdrant helpers ────────────────────────────────────────────────────────────

def ensure_collection(qdrant: QdrantClient, dim: int):
    existing = {c.name for c in qdrant.get_collections().collections}
    if QDRANT_COLLECTION not in existing:
        qdrant.create_collection(
            collection_name=QDRANT_COLLECTION,
            vectors_config=VectorParams(size=dim, distance=Distance.COSINE),
        )
        print(f"  ✔ Created Qdrant collection '{QDRANT_COLLECTION}'")
    else:
        print(f"  ✔ Using existing Qdrant collection '{QDRANT_COLLECTION}'")


def upsert_points(qdrant: QdrantClient, points: list[PointStruct]):
    qdrant.upsert(collection_name=QDRANT_COLLECTION, points=points)


# ── Main pipeline ─────────────────────────────────────────────────────────────

def process(source_path: str):
    source = Path(source_path).resolve()
    if not source.exists():
        print(f"[ERROR] Path does not exist: {source}")
        sys.exit(1)

    print("\n" + "═" * 60)
    print("  Document Embedding Tool")
    print("═" * 60)
    print(f"  Source     : {source}")
    print(f"  LM Studio  : {LM_STUDIO_BASE_URL}")
    print(f"  Model      : {EMBEDDING_MODEL}")
    print(f"  Qdrant     : {QDRANT_HOST}:{QDRANT_PORT}")
    print("═" * 60 + "\n")

    # ── Step 1: Collect files ────────────────────────────────────────────────
    t_start = time.perf_counter()

    print("📂 Collecting files …")
    files = collect_files(source)
    if not files:
        print(f"[WARN] No supported documents found ({', '.join(SUPPORTED_EXTENSIONS)}).")
        sys.exit(0)
    print(f"  Found {len(files)} file(s)\n")

    # ── Step 2: Connect to services ─────────────────────────────────────────
    print("🔌 Connecting to LM Studio and Qdrant …")
    try:
        lm_client, qdrant_client = build_clients()
    except Exception as exc:
        print(f"[ERROR] Could not create clients: {exc}")
        sys.exit(1)

    # Probe LM Studio with a tiny embedding to detect dimension dynamically
    try:
        probe = embed_texts(lm_client, ["probe"])
        actual_dim = len(probe[0])
    except Exception as exc:
        print(f"[ERROR] LM Studio embedding probe failed: {exc}")
        sys.exit(1)
    print(f"  Embedding dimension detected: {actual_dim}\n")

    try:
        ensure_collection(qdrant_client, actual_dim)
    except Exception as exc:
        print(f"[ERROR] Qdrant collection setup failed: {exc}")
        sys.exit(1)

    # ── Step 3: Process each file ────────────────────────────────────────────
    total_chunks = 0
    total_vectors_stored = 0

    t_embed_total = 0.0

    print("\n📄 Processing documents …\n")
    for file_path in tqdm(files, desc="Files", unit="file"):
        try:
            text = load_document(file_path)
        except Exception as exc:
            tqdm.write(f"  [SKIP] {file_path.name}: {exc}")
            continue

        chunks = chunk_text(text)
        if not chunks:
            tqdm.write(f"  [SKIP] {file_path.name}: empty after loading")
            continue

        total_chunks += len(chunks)
        points: list[PointStruct] = []

        t_e0 = time.perf_counter()
        try:
            vectors = embed_texts(lm_client, chunks)
        except Exception as exc:
            tqdm.write(f"  [ERROR] Embedding failed for {file_path.name}: {exc}")
            continue
        t_embed_total += time.perf_counter() - t_e0

        for chunk_text_val, vector in zip(chunks, vectors):
            points.append(
                PointStruct(
                    id=str(uuid.uuid4()),
                    vector=vector,
                    payload={
                        "source": str(file_path),
                        "filename": file_path.name,
                        "text": chunk_text_val,
                    },
                )
            )

        try:
            upsert_points(qdrant_client, points)
            total_vectors_stored += len(points)
        except Exception as exc:
            tqdm.write(f"  [ERROR] Qdrant upsert failed for {file_path.name}: {exc}")

    t_total = time.perf_counter() - t_start

    # ── Step 4: Summary ──────────────────────────────────────────────────────
    print("\n" + "═" * 60)
    print("  ✅  Summary")
    print("═" * 60)
    print(f"  Files processed       : {len(files)}")
    print(f"  Total chunks created  : {total_chunks}")
    print(f"  Vectors stored        : {total_vectors_stored}")
    print(f"  Embedding time        : {t_embed_total:.2f}s")
    print(f"  Total elapsed time    : {t_total:.2f}s")
    if total_chunks:
        avg_ms = (t_embed_total / total_chunks) * 1000
        print(f"  Avg time per chunk    : {avg_ms:.1f}ms")
    print("═" * 60 + "\n")


# ── Entry point ───────────────────────────────────────────────────────────────

def main():
    parser = argparse.ArgumentParser(
        description="Embed documents and store vectors in Qdrant via LM Studio."
    )
    parser.add_argument(
        "source",
        help="Path to a document file or a directory containing documents.",
    )
    args = parser.parse_args()
    process(args.source)


if __name__ == "__main__":
    main()
