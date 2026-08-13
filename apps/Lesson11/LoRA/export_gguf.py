from unsloth import FastLanguageModel

# 1. Eğitilmiş LoRA adaptörünü (ve temel modeli) yüklüyoruz
max_seq_length = 2048
load_in_4bit = True

model, tokenizer = FastLanguageModel.from_pretrained(
    model_name = "lora_model_sonuc",  # trainer.py'nin ürettiği adaptör klasörü
    max_seq_length = max_seq_length,
    load_in_4bit = load_in_4bit,
)

# 2. LoRA ağırlıklarını temel modelle birleştirip GGUF formatında dışa aktarıyoruz
#    (LM Studio / Ollama gibi araçlarda doğrudan kullanılabilmesi için)
print("--- GGUF Dışa Aktarımı Başlıyor ---")
model.save_pretrained_gguf(
    "llama3-8b-ai-lecture-gguf",
    tokenizer,
    quantization_method = "q4_k_m",  # boyut/kalite dengesi iyi bir seçenek
)
print("İşlem tamamlandı! 'llama3-8b-ai-lecture-gguf' klasöründeki .gguf dosyasını LM Studio'ya import edin.")
