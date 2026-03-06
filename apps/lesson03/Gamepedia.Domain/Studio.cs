namespace Gamepedia.Domain;

public class Studio
{
    public int StudioId { get; }
    public required string Name { get; set; }
    public IEnumerable<Game> Games { get; set; } = [];
    // Daha birçok özellik eklenebilir, örneğin: Kuruluş Yılı, Kurucular
}
