namespace ProjectRouter.Data;

/// <summary>A row of an ordered child list (participant_preferences / project_items).</summary>
internal sealed class ItemRow
{
    public string OwnerId { get; set; } = "";
    public string Kind { get; set; } = "";
    public long Rank { get; set; }
    public string Name { get; set; } = "";

    public static IEnumerable<ItemRow> Ranked(string ownerId, string kind, IEnumerable<string> names) =>
        names.Select((name, index) => new ItemRow { OwnerId = ownerId, Kind = kind, Rank = index, Name = name });
}
