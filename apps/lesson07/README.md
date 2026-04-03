# Document Embedding Tool

A terminal application that reads documents, converts them into vectors using **LM Studio** text embeddings, and stores them in **Qdrant** vector database.

## Prerequisites

| Service | URL |
|---------|-----|
| LM Studio | http://127.0.0.1:1234 |
| Qdrant (Docker) | http://localhost:6333 |

Make sure both services are running before executing the tool.  
Load the `text-embedding-embeddinggemma-300m` model in LM Studio.

## Setup

```bash
pip install -r requirements.txt
```

## Usage

```bash
# Embed a single file
python main.py path/to/document.pdf

# Embed all documents in a directory (recursive)
python main.py path/to/docs/
```

## Supported file types

| Extension | Description |
|-----------|-------------|
| `.txt` | Plain text |
| `.md` | Markdown |
| `.pdf` | PDF documents |
| `.docx` | Word documents |

## Output example

```
════════════════════════════════════════════════════════════
  Document Embedding Tool
════════════════════════════════════════════════════════════
  Source     : /path/to/docs
  LM Studio  : http://127.0.0.1:1234/v1
  Model      : text-embedding-embeddinggemma-300m
  Qdrant     : localhost:6333
════════════════════════════════════════════════════════════

📂 Collecting files …
  Found 3 file(s)

🔌 Connecting to LM Studio and Qdrant …
  Embedding dimension detected: 768
  ✔ Using existing Qdrant collection 'documents'

📄 Processing documents …
Files: 100%|████████████████| 3/3 [00:12<00:00]

════════════════════════════════════════════════════════════
  ✅  Summary
════════════════════════════════════════════════════════════
  Files processed       : 3
  Total chunks created  : 47
  Vectors stored        : 47
  Embedding time        : 11.43s
  Total elapsed time    : 12.07s
  Avg time per chunk    : 243.2ms
════════════════════════════════════════════════════════════
```
