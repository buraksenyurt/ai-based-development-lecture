"""
RAG Chatbot
Uses documents previously indexed in Qdrant (by lesson07) to answer questions
via a Retrieval-Augmented Generation pipeline backed by LM Studio.
"""

import argparse
import sys

from openai import OpenAI
from qdrant_client import QdrantClient

# ── Configuration ──────────────────────────────────────────────────────────────

LM_STUDIO_BASE_URL = "http://127.0.0.1:1234/v1"
EMBEDDING_MODEL    = "text-embedding-embeddinggemma-300m"
CHAT_MODEL         = "local-model"          # change to the model loaded in LM Studio

QDRANT_HOST       = "localhost"
QDRANT_PORT       = 7333
QDRANT_COLLECTION = "documents"

TOP_K              = 5                      # number of chunks to retrieve per query

SYSTEM_PROMPT = """You are a helpful assistant that answers questions based on the
provided document context. Use ONLY the information in the context to answer.
If the context does not contain enough information, say so clearly.
Always cite the source file name for claims you make."""

# ── Clients ────────────────────────────────────────────────────────────────────

def build_clients() -> tuple[OpenAI, QdrantClient]:
    lm     = OpenAI(base_url=LM_STUDIO_BASE_URL, api_key="lm-studio")
    qdrant = QdrantClient(host=QDRANT_HOST, port=QDRANT_PORT)
    return lm, qdrant


# ── Embedding ──────────────────────────────────────────────────────────────────

def embed_query(lm_client: OpenAI, text: str) -> list[float]:
    response = lm_client.embeddings.create(model=EMBEDDING_MODEL, input=[text])
    return response.data[0].embedding


# ── Retrieval ──────────────────────────────────────────────────────────────────

def retrieve_context(
    qdrant_client: QdrantClient,
    vector: list[float],
    top_k: int,
) -> list[dict]:
    response = qdrant_client.query_points(
        collection_name=QDRANT_COLLECTION,
        query=vector,
        limit=top_k,
        with_payload=True,
    )
    return [
        {
            "text":     hit.payload.get("text", ""),
            "filename": hit.payload.get("filename", "unknown"),
            "source":   hit.payload.get("source", ""),
            "score":    round(hit.score, 4),
        }
        for hit in response.points
    ]


def format_context(chunks: list[dict]) -> str:
    if not chunks:
        return "No relevant context found."
    parts = []
    for i, chunk in enumerate(chunks, 1):
        parts.append(
            f"[{i}] Source: {chunk['filename']} (score: {chunk['score']})\n"
            f"{chunk['text'].strip()}"
        )
    return "\n\n".join(parts)


# ── RAG turn ───────────────────────────────────────────────────────────────────

def ask(
    lm_client: OpenAI,
    qdrant_client: QdrantClient,
    history: list[dict],
    user_message: str,
    top_k: int,
    chat_model: str,
) -> tuple[str, list[dict]]:
    """
    Performs one RAG turn:
    1. Embed query → retrieve context
    2. Build augmented system message
    3. Call chat completions with history
    Returns (assistant_reply, retrieved_chunks).
    """
    vector = embed_query(lm_client, user_message)
    chunks = retrieve_context(qdrant_client, vector, top_k)
    context_text = format_context(chunks)

    augmented_system = (
        f"{SYSTEM_PROMPT}\n\n"
        f"--- CONTEXT ---\n{context_text}\n--- END CONTEXT ---"
    )

    messages = [{"role": "system", "content": augmented_system}]
    messages.extend(history)
    messages.append({"role": "user", "content": user_message})

    response = lm_client.chat.completions.create(
        model=chat_model,
        messages=messages,
    )
    reply = response.choices[0].message.content.strip()
    return reply, chunks


# ── CLI ────────────────────────────────────────────────────────────────────────

def print_banner(chat_model: str, top_k: int):
    print("\n" + "═" * 60)
    print("  RAG Chatbot")
    print("═" * 60)
    print(f"  LM Studio  : {LM_STUDIO_BASE_URL}")
    print(f"  Chat model : {chat_model}")
    print(f"  Embed model: {EMBEDDING_MODEL}")
    print(f"  Qdrant     : {QDRANT_HOST}:{QDRANT_PORT}  collection='{QDRANT_COLLECTION}'")
    print(f"  Top-K      : {top_k}")
    print("═" * 60)
    print("  Commands:  /exit  — quit   |  /clear — reset history")
    print("═" * 60 + "\n")


def main():
    parser = argparse.ArgumentParser(
        description="RAG chatbot powered by LM Studio and Qdrant."
    )
    parser.add_argument(
        "--top-k",
        type=int,
        default=TOP_K,
        help=f"Number of chunks to retrieve per query (default: {TOP_K}).",
    )
    parser.add_argument(
        "--model",
        default=CHAT_MODEL,
        help=f"LM Studio chat model identifier (default: {CHAT_MODEL}).",
    )
    parser.add_argument(
        "--show-sources",
        action="store_true",
        default=False,
        help="Print retrieved source file names after each answer.",
    )
    args = parser.parse_args()

    print_banner(args.model, args.top_k)

    # ── Connect ────────────────────────────────────────────────────────────
    print("🔌 Connecting to LM Studio and Qdrant …")
    try:
        lm_client, qdrant_client = build_clients()
    except Exception as exc:
        print(f"[ERROR] Could not create clients: {exc}")
        sys.exit(1)

    # Verify embedding endpoint with a probe
    try:
        embed_query(lm_client, "probe")
        print("  ✔ LM Studio embedding endpoint reachable")
    except Exception as exc:
        print(f"[ERROR] LM Studio embedding probe failed: {exc}")
        sys.exit(1)

    # Verify Qdrant collection exists
    try:
        existing = {c.name for c in qdrant_client.get_collections().collections}
        if QDRANT_COLLECTION not in existing:
            print(
                f"[WARN] Qdrant collection '{QDRANT_COLLECTION}' not found. "
                "Run lesson07/main.py to index documents first."
            )
        else:
            print(f"  ✔ Qdrant collection '{QDRANT_COLLECTION}' found")
    except Exception as exc:
        print(f"[ERROR] Could not reach Qdrant: {exc}")
        sys.exit(1)

    print("\nReady. Type your question below.\n")

    # ── Chat loop ──────────────────────────────────────────────────────────
    history: list[dict] = []

    while True:
        try:
            user_input = input("You: ").strip()
        except (EOFError, KeyboardInterrupt):
            print("\nBye!")
            break

        if not user_input:
            continue

        if user_input.lower() == "/exit":
            print("Bye!")
            break

        if user_input.lower() == "/clear":
            history.clear()
            print("  [History cleared]\n")
            continue

        try:
            reply, chunks = ask(
                lm_client,
                qdrant_client,
                history,
                user_input,
                args.top_k,
                args.model,
            )
        except Exception as exc:
            print(f"[ERROR] {exc}\n")
            continue

        print(f"\nAssistant: {reply}\n")

        if args.show_sources:
            sources = list({c["filename"] for c in chunks if c["filename"] != "unknown"})
            if sources:
                print(f"  Sources: {', '.join(sorted(sources))}\n")

        # Append to history for multi-turn context
        history.append({"role": "user",      "content": user_input})
        history.append({"role": "assistant", "content": reply})


if __name__ == "__main__":
    main()
