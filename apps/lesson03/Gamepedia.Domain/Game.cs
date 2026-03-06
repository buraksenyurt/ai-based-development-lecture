namespace Gamepedia.Domain;

/*
    Örnek veri :

    Red Alert II, 
    2000, 
    Strategy,
    Command & Conquer: Red Alert 2, Westwood Studios tarafından geliştirilen ve Electronic Arts tarafından yayınlanan gerçek zamanlı strateji oyunudur. 
    7.6
*/
public class Game
{
    public int GameId { get; }
    public Genre Genre { get; set; }
    public required string Title { get; set; }
    public short ReleaseYear { get; set; }
    public string Summary { get; set; }
    public float Rating { get; set; }
    public required Studio Studio { get; set; }
}
