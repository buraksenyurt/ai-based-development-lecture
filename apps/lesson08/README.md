# Lesson 08 — RAG Chatbot

An interactive terminal chatbot that answers questions using a **Retrieval-Augmented Generation (RAG)** pipeline.

Documents are retrieved from the **Qdrant** vector database (indexed by [lesson07](../lesson07/)) and injected as context into a chat completion request sent to **LM Studio**.

---

## Prerequisites

| Requirement | Details |
|---|---|
| **Qdrant** running | `docker compose up qdrant` from the repo root |
| **Documents indexed** | Run `lesson07/main.py` to embed and store documents first |
| **LM Studio** running | Start the local server; load an embedding model **and** a chat model |
| **Embedding model** | `text-embedding-embeddinggemma-300m` (same as lesson07) |
| **Chat model** | Any instruction-following model loaded in LM Studio |

---

## Setup

```bash
cd apps/lesson08
pip install -r requirements.txt
```

---

## Configuration

Open `main.py` and update the constants at the top if needed:

| Constant | Default | Description |
|---|---|---|
| `LM_STUDIO_BASE_URL` | `http://127.0.0.1:1234/v1` | LM Studio server URL |
| `CHAT_MODEL` | `local-model` | Model identifier shown in LM Studio |
| `EMBEDDING_MODEL` | `text-embedding-embeddinggemma-300m` | Must match the model used during indexing |
| `QDRANT_HOST` / `QDRANT_PORT` | `localhost` / `7333` | Qdrant connection |
| `QDRANT_COLLECTION` | `documents` | Must match the collection used in lesson07 |
| `TOP_K` | `5` | Number of chunks retrieved per query |

---

## Usage

```bash
# Basic — starts the interactive chat loop
python main.py

# Override top-K retrieved chunks
python main.py --top-k 8

# Specify a different LM Studio chat model
python main.py --model "qwen/qwen3-14bs"

# Show source file names after each answer
python main.py --show-sources
```

### In-session commands

| Command | Action |
|---|---|
| `/exit` | Quit the chatbot |
| `/clear` | Reset conversation history |

---

## Example session

```
============================================================
  RAG Chatbot
============================================================
  LM Studio  : http://127.0.0.1:1234/v1
  Chat model : local-model
  Embed model: text-embedding-embeddinggemma-300m
  Qdrant     : localhost:7333  collection='documents'
  Top-K      : 5
============================================================
  Commands:  /exit  — quit   |  /clear — reset history
============================================================

You: What is the repository pattern?