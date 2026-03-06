namespace Gamepedia.Domain;

/*
    Domain — İş alanının temel kavramlarını temsil eder.
    Domain katmanı hiçbir dış katmana (Data, Application, UI) bağımlı değildir/olmamamlıdır.
    Bu sayede iş kuralları bağımsız olarak test edilebilir ve taşınabilir.

    Örnek veri :

    Red Alert II, 
    2000, 
    Strategy,
    Command & Conquer: Red Alert 2, Westwood Studios tarafından geliştirilen ve Electronic Arts tarafından yayınlanan gerçek zamanlı strateji oyunudur. 
    7.6

    Game bir "Domain Entity"dir: kimliği (GameId) ile ayırt edilir, değerleriyle değil.
    İki oyunun tüm alanları aynı olsa bile GameId'leri farklıysa bunlar farklı nesnelerdir.
    Value Object örneği için karşılaştırın: iki Genre.Action değeri her zaman eşittir.
*/
public class Game
{
    // GameId yalnızca get içerir — dışarıdan atanamaz. Kimlik değişmez (immutable identity).
    // Gerçek projede bu değer genellikle veri tabanı tarafından atanır (auto-increment veya GUID).
    public int GameId { get; }

    public Genre Genre { get; set; }

    // 'required' (C# 11): Nesne oluşturulurken bu alanın set edilmesi zorunludur.
    // Derleyici, new Game { } yapısında Title verilmezse derleme hatası üretir.
    public required string Title { get; set; }

    public short ReleaseYear { get; set; }

    // DIKKAT: Rating için float kullanıldı. Parasal değerler için decimal tercih edilmeli.
    // float/double ikili (binary) kesirli sayıları tam temsil edemez; puanlama için kabul edilebilir.
    public float Rating { get; set; }

    public string Summary { get; set; }

    public required Studio Studio { get; set; }
}
