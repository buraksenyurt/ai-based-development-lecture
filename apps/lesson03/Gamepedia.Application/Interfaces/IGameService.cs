using Gamepedia.Domain;

namespace Gamepedia.Application.Interfaces;

/*
Application Service Arayüzü

Application katmanı iş akışlarını (use case) orkestre etmek için kullanılabilir. Örneğin;
  - Validasyon işlemleri yapar
  - Repository'leri çağırır
  - Gerekirse olaylar (event) tetikler. Sistme yeni bir oyun eklendi, bir oyunun puanı değişti gibi.

 IGameService servisi de bir arayüzle soyutlanır; böylece:
  - Farklı UI katmanları (API, Console, WPF) aynı IGameService'i kullanabilir.
  - Özellikle birim testlerde ilgili servis mocklanabilir.
*/
public interface IGameService
{
    void RegisterGame(Game game);
    void UpdateGameRating(int gameId, float newRating);
}
