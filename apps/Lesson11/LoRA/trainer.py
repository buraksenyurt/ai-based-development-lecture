import json
import torch
from unsloth import FastLanguageModel
from datasets import Dataset
from trl import SFTTrainer
from transformers import TrainingArguments

# 1. Modelle ilgili konfigürasyon ayarlamaları
max_seq_length = 2048  # Modelin işleyeceği maksimum metin uzunluğu
dtype = None           # GPU türüne göre otomatik seçilir (Float16 veya Bfloat16)
load_in_4bit = True    # Bellek dostu olması için 4-bit yükleme aktif

# Hugging Face'ten optimize edilmiş temel modeli yüklüyoruz
model, tokenizer = FastLanguageModel.from_pretrained(
    model_name = "unsloth/llama-3-8b-Instruct-bnb-4bit",
    max_seq_length = max_seq_length,
    dtype = dtype,
    load_in_4bit = load_in_4bit,
)

# 2. Modeli LoRA eğitimi için hazırlama (Ağırlıkları dondurma ve katman ekleme)
model = FastLanguageModel.get_peft_model(
    model,
    r = 16,                # LoRA Rank (Büyük değer = daha çok bellek, daha derin öğrenme)
    target_modules = ["q_proj", "k_proj", "v_proj", "o_proj", "gate_proj", "up_proj", "down_proj"],
    lora_alpha = 16,
    lora_dropout = 0,      # Performans için 0 olarak optimize edilmiştir
    bias = "none",         
    use_gradient_checkpointing = "unsloth", # VRAM tasarrufu sağlar
    random_state = 3407,
)

# 3. Yerel veri setini tanımlama (Deney için basit Alpaca formatı)
alpaca_prompt = """Aşağıda bir görevi tanımlayan bir talimat yer almaktadır. Talebi uygun şekilde karşılayan bir yanıt yazın.

### Talimat:
{}

### Yanıt:
{}"""

# Eğitim örneklerini dataSet.json dosyasından okuyoruz
with open("dataSet.json", "r", encoding="utf-8") as f:
    data = json.load(f)

# Verileri modelin anlayacağı şablona dönüştürüyoruz
def formatting_prompts_func(examples):
    instructions = examples["instruction"]
    outputs      = examples["output"]
    texts = []
    for instruction, output in zip(instructions, outputs):
        text = alpaca_prompt.format(instruction, output) + tokenizer.eos_token
        texts.append(text)
    return { "text" : texts }

dataset = Dataset.from_dict(data)
dataset = dataset.map(formatting_prompts_func, batched = True)

# 4. Eğitim Parametreleri (Training Arguments)
trainer = SFTTrainer(
    model = model,
    tokenizer = tokenizer,
    train_dataset = dataset,
    dataset_text_field = "text",
    max_seq_length = max_seq_length,
    dataset_num_proc = 2,
    packing = False, # Küçük veri setleri için False olmalı
    args = TrainingArguments(
        per_device_train_batch_size = 2,
        gradient_accumulation_steps = 4,
        warmup_steps = 5,
        max_steps = 60, # Deney için adım sayısını kısa tuttuk
        learning_rate = 2e-4,
        fp16 = not torch.cuda.is_bf16_supported(),
        bf16 = torch.cuda.is_bf16_supported(),
        logging_steps = 1,
        output_dir = "outputs",
    ),
)

# 5. Eğitimin Başlatılması
print("--- LoRA Eğitimi Başlıyor ---")
trainer_stats = trainer.train()

# 6. Sadece LoRA Ağırlıklarını Yerel Diske Kaydetme
print("--- LoRA Modeli Kaydediliyor ---")
model.save_pretrained("lora_model_sonuc")
tokenizer.save_pretrained("lora_model_sonuc")
print("İşlem tamamlandı! 'lora_model_sonuc' klasörünü kontrol edin.")
