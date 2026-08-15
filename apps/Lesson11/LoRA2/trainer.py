import random
import torch
from unsloth import FastLanguageModel
from unsloth.chat_templates import get_chat_template, train_on_responses_only
from datasets import load_dataset, Dataset
from trl import SFTTrainer
from transformers import TrainingArguments

random.seed(3407)

# ------------------------------------------------------------------
# 0. Temel Model Seçimi
# ------------------------------------------------------------------
# "llama-3.1" veya "qwen-2.5" olarak değiştirerek iki farklı temel model
# arasında geçiş yapabilirsiniz. instruction_part/response_part değerleri
# her modelin kendi sohbet şablonundaki kullanıcı/asistan sınırlarını
# birebir yansıtmalıdır(train_on_responses_only bu string'leri arar).
MODEL_CONFIGS = {
    "llama-3.1": {
        "model_name": "unsloth/Meta-Llama-3.1-8B-Instruct-bnb-4bit",
        "chat_template": "llama-3.1",
        "instruction_part": "<|start_header_id|>user<|end_header_id|>\n\n",
        "response_part": "<|start_header_id|>assistant<|end_header_id|>\n\n",
    },
    "qwen-2.5": {
        "model_name": "unsloth/Qwen2.5-7B-Instruct-bnb-4bit",
        "chat_template": "qwen-2.5",
        "instruction_part": "<|im_start|>user\n",
        "response_part": "<|im_start|>assistant\n",
    },
}
BASE_MODEL_CHOICE = "llama-3.1"
config = MODEL_CONFIGS[BASE_MODEL_CHOICE]

# Kaç oyun kullanılacağı ve oyun başına en fazla kaç soru-cevap üretileceği.
# Bu iki değer toplam eğitim örneği sayısını (dolayısıyla RunPod'daki GPU
# süresini) doğrudan belirler.
GAME_COUNT = 150
MAX_QA_PER_GAME = 4

# ------------------------------------------------------------------
# 1. Modelle ilgili konfigürasyon ayarlamaları
# ------------------------------------------------------------------
max_seq_length = 2048  # Modelin işleyeceği maksimum metin uzunluğu
dtype = None            # GPU türüne göre otomatik seçilir(Float16 veya Bfloat16)
load_in_4bit = True     # Bellek dostu olması için 4-bit yükleme aktif

model, tokenizer = FastLanguageModel.from_pretrained(
    model_name = config["model_name"],
    max_seq_length = max_seq_length,
    dtype = dtype,
    load_in_4bit = load_in_4bit,
)

# Model Instruct/chat tabanlı olduğu için eğitimi de kendi sohbet şablonuyla
# hizalıyoruz; aksi halde LM Studio gibi araçlar çıkarım sırasında farklı bir
# şablon uygular ve model eğitimde görmediği bir formatla karşılaşır.
tokenizer = get_chat_template(
    tokenizer,
    chat_template = config["chat_template"],
)

# ------------------------------------------------------------------
# 2. Modeli LoRA eğitimi için hazırlama(Ağırlıkları dondurma ve katman ekleme)
# ------------------------------------------------------------------
# Bu örnekte amaç LoRA/trainer.py'deki gibi belirli gerçekleri(bu kez oyun
# bilgilerini) ezberletmek; bu nedenle r/lora_alpha değerleri, LoRA2'nin ilk
# sürümünde(genel talimat takibi) kullanılan 16/16 yerine, LoRA/trainer.py'de
# olduğu gibi 32/32 olarak tutuldu.
model = FastLanguageModel.get_peft_model(
    model,
    r = 32,                # LoRA Rank (Büyük değer = daha çok bellek, daha derin öğrenme)
    target_modules = ["q_proj", "k_proj", "v_proj", "o_proj", "gate_proj", "up_proj", "down_proj"],
    lora_alpha = 32,
    lora_dropout = 0,      # Performans için 0 olarak optimize edilmiştir
    bias = "none",
    use_gradient_checkpointing = "unsloth", # VRAM tasarrufu sağlar
    random_state = 3407,
)

# ------------------------------------------------------------------
# 3. Steam Games veri setini indirme ve soru-cevap çiftlerine dönüştürme
# ------------------------------------------------------------------
# Z02Z/steam-games-dataset: FronkonGames/steam-games-dataset'in bir kopyası.
# ~123.000 satır; her satır bir oyunu ve tür, geliştirici, fiyat, platform,
# açıklama gibi yapılandırılmış(tabular) alanlarını içerir. Bu veri seti
# hazır instruction/output çiftleri içermez ve bu yüzden aşağıda kendi soru-
# cevap çiftlerimizi bu alanlardan programatik olarak üretiyoruz.
raw_dataset = load_dataset("Z02Z/steam-games-dataset", split = "train")

def is_missing(value):
    if value is None:
        return True
    if isinstance(value, str) and value.strip() == "":
        return True
    if isinstance(value, (list, tuple)) and len(value) == 0:
        return True
    return False

def has_essential_fields(row):
    return (
        not is_missing(row.get("name"))
        and not is_missing(row.get("genres"))
        and not is_missing(row.get("short_description"))
    )

# Eksik temel alanlara sahip satırları eleyip, en çok olumlu yoruma(positive)
# sahip(yani en tanınmış) oyunları seçiyoruz. Böylece LM Studio'da elle test
# ederken sonuçları değerlendirmek de kolaylaşır.
raw_dataset = raw_dataset.filter(has_essential_fields)
raw_dataset = raw_dataset.sort("positive", reverse = True)
selected_games = raw_dataset.select(range(GAME_COUNT))

def format_platforms(row):
    platforms = []
    if row.get("windows"):
        platforms.append("Windows")
    if row.get("mac"):
        platforms.append("macOS")
    if row.get("linux"):
        platforms.append("Linux")
    return platforms

def build_qa_pairs(row):
    name = row["name"]
    pairs = []

    if not is_missing(row.get("genres")):
        pairs.append((
            f"What genre(s) does {name} belong to?",
            f"{name} belongs to the following genre(s): {', '.join(row['genres'])}.",
        ))

    if not is_missing(row.get("developers")):
        pairs.append((
            f"Who developed {name}?",
            f"{name} was developed by {', '.join(row['developers'])}.",
        ))

    if not is_missing(row.get("publishers")):
        pairs.append((
            f"Who published {name}?",
            f"{name} was published by {', '.join(row['publishers'])}.",
        ))

    if not is_missing(row.get("release_date")):
        pairs.append((
            f"When was {name} released?",
            f"{name} was released on {row['release_date']}.",
        ))

    price = row.get("price")
    if price is not None and price == price:  # NaN kontrolü
        if price == 0:
            pairs.append((f"How much does {name} cost?", f"{name} is free to play."))
        else:
            pairs.append((f"How much does {name} cost?", f"{name} costs ${price:.2f}."))

    platforms = format_platforms(row)
    if platforms:
        pairs.append((
            f"Which platforms support {name}?",
            f"{name} is available on {', '.join(platforms)}.",
        ))

    if not is_missing(row.get("short_description")):
        pairs.append((f"Give a short description of {name}.", row["short_description"].strip()))

    score = row.get("metacritic_score")
    if score and score > 0:
        pairs.append((f"What is the Metacritic score of {name}?", f"{name} has a Metacritic score of {score}."))

    random.shuffle(pairs)
    return pairs[:MAX_QA_PER_GAME]

instructions, outputs = [], []
for row in selected_games:
    for instruction, output in build_qa_pairs(row):
        instructions.append(instruction)
        outputs.append(output)

print(f"--- {GAME_COUNT} oyundan toplam {len(instructions)} soru-cevap çifti üretildi ---")

# Verileri modelin kendi sohbet şablonuna göre dönüştürüyoruz.
def formatting_prompts_func(examples):
    texts = []
    for instruction, output in zip(examples["instruction"], examples["output"]):
        convo = [
            {"role": "user", "content": instruction},
            {"role": "assistant", "content": output},
        ]
        text = tokenizer.apply_chat_template(convo, tokenize = False, add_generation_prompt = False)
        texts.append(text)
    return { "text" : texts }

dataset = Dataset.from_dict({"instruction": instructions, "output": outputs})
dataset = dataset.map(formatting_prompts_func, batched = True, remove_columns = dataset.column_names)

# ------------------------------------------------------------------
# 4. Eğitim Parametreleri(Training Arguments)
# ------------------------------------------------------------------
trainer = SFTTrainer(
    model = model,
    tokenizer = tokenizer,
    train_dataset = dataset,
    dataset_text_field = "text",
    max_seq_length = max_seq_length,
    dataset_num_proc = 2,
    packing = False,
    args = TrainingArguments(
        per_device_train_batch_size = 2,
        gradient_accumulation_steps = 4,
        warmup_steps = 5,
        max_steps = 300,
        learning_rate = 2e-4,
        fp16 = not torch.cuda.is_bf16_supported(),
        bf16 = torch.cuda.is_bf16_supported(),
        logging_steps = 1,
        output_dir = "outputs",
    ),
)

# Kaybı(loss) sadece asistan yanıtı üzerinden hesaplıyoruz. Böylece model
# soruyu tekrar üretmeyi değil doğru cevabı üretmeyi öğrenir.
trainer = train_on_responses_only(
    trainer,
    instruction_part = config["instruction_part"],
    response_part = config["response_part"],
)

# ------------------------------------------------------------------
# 5. Eğitimin Başlatılması
# ------------------------------------------------------------------
print("--- LoRA Eğitimi Başlıyor ---")
trainer_stats = trainer.train()
print(f"--- Eğitim tamamlandı: {trainer_stats.metrics['train_runtime']:.2f} sn, "
      f"ortalama eğitim kaybı (loss): {trainer_stats.metrics['train_loss']:.4f} ---")

# ------------------------------------------------------------------
# 6. Sadece LoRA Ağırlıklarını Yerel Diske Kaydetme
# ------------------------------------------------------------------
print("--- LoRA Modeli Kaydediliyor ---")
model.save_pretrained("lora_model_sonuc")
tokenizer.save_pretrained("lora_model_sonuc")
print("İşlem tamamlandı! 'lora_model_sonuc' klasörünü kontrol edin.")

# ------------------------------------------------------------------
# 7. Hızlı Doğrulama(Sanity Check)
# ------------------------------------------------------------------
# GGUF'a aktarıp LM Studio'da denemeden önce, adaptörün eğitim verisindeki
# gerçekleri gerçekten öğrenip öğrenmediğini burada, aynı process içinde,
# greedy(do_sample=False) decoding ile kontrol ediyoruz. Sorular, üretilen
# veri setinden rastgele seçiliyor; böylece hem soru hem de beklenen(dataset)
# yanıt eğitimde gerçekten kullanılmış oluyor ve model yanıtıyla doğrudan
# karşılaştırılabiliyor.
print("--- Hızlı Doğrulama Başlıyor ---")
FastLanguageModel.for_inference(model)
sanity_sample_count = min(5, len(instructions))
sanity_indices = random.sample(range(len(instructions)), sanity_sample_count)
for i in sanity_indices:
    question = instructions[i]
    expected = outputs[i]
    convo = [{"role": "user", "content": question}]
    inputs = tokenizer.apply_chat_template(
        convo,
        tokenize = True,
        add_generation_prompt = True,
        return_tensors = "pt",
    ).to("cuda")
    generated = model.generate(input_ids = inputs, max_new_tokens = 150, do_sample = False, use_cache = True)
    response = tokenizer.batch_decode(generated[:, inputs.shape[1]:], skip_special_tokens = True)[0]
    print(f"Soru: {question}\nBeklenen (dataset) Yanıt: {expected}\nModel Yanıtı: {response}\n{'-' * 40}")
