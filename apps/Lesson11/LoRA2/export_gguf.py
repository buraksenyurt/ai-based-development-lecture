from unsloth import FastLanguageModel

max_seq_length = 2048
load_in_4bit = True

model, tokenizer = FastLanguageModel.from_pretrained(
    model_name = "lora_model_sonuc",
    max_seq_length = max_seq_length,
    load_in_4bit = load_in_4bit,
)

print("--- GGUF Dışa Aktarımı Başlıyor ---")
model.save_pretrained_gguf(
    "steam-games-lora-gguf",
    tokenizer,
    quantization_method = "q4_k_m",
)
print("İşlem tamamlandı! 'steam-games-lora-gguf' klasöründeki .gguf dosyasını LM Studio'ya import edin.")
