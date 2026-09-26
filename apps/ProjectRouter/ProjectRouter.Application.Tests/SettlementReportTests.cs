using System.Text;
using ProjectRouter.Application.Reports;
using ProjectRouter.Application.Services;
using ProjectRouter.Domain;

namespace ProjectRouter.Application.Tests;

public class SettlementReportTests
{
    private readonly ProjectIdea _arena = TestData.Project(["C#"], ["SQLite"], 1, 3, title: "Arena");
    private readonly ProjectIdea _closed = TestData.Project(["Go"], ["Redis"], 5, 6, title: "Büyük Proje");
    private readonly Participant _ada = TestData.Participant(["C#"], ["SQLite"], name: "Ada Yılmaz");
    private readonly Participant _bob = TestData.Participant(["Python"], ["Redis"], name: "Bob; \"Jr\"");
    private readonly Participant _cem = TestData.Participant(["Go"], ["Redis"], name: "=Cem");

    private CompetitionDetails Details(bool settled = true)
    {
        var competition = new Competition(Guid.NewGuid(), "AI <Cup>", "2026-27", [_arena.Id, _closed.Id], [_ada.Id, _bob.Id, _cem.Id]);
        if (settled)
        {
            competition.AssignSettlement(
                new Dictionary<Guid, IReadOnlyList<Guid>> { [_arena.Id] = [_ada.Id, _bob.Id] },
                [_arena, _closed],
                new DateTime(2026, 9, 26, 9, 30, 0, DateTimeKind.Utc));
        }

        return new CompetitionDetails
        {
            Competition = competition,
            Participants = [_ada, _bob, _cem],
            Projects = [_arena, _closed],
            Teams = settled ? [new Team(_arena, [new TeamMember(_ada, 1.0), new TeamMember(_bob, 0.3)])] : [],
            UnassignedParticipants = settled ? [_cem] : [],
            ClosedProjects = settled ? [_closed] : [],
        };
    }

    [Theory]
    [InlineData(1.0, MatchLevel.Strong)]
    [InlineData(0.75, MatchLevel.Strong)]
    [InlineData(0.5, MatchLevel.Partial)]
    [InlineData(0.0, MatchLevel.None)]
    public void MatchLevels_Classify_UsesThresholds(double score, MatchLevel expected)
    {
        Assert.Equal(expected, MatchLevels.Classify(score));
    }

    [Fact]
    public void Csv_HasHeaderAndOneRowPerParticipant()
    {
        var lines = SettlementCsv.Write(Details()).Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

        Assert.Equal(4, lines.Length);
        Assert.StartsWith("Proje;Takım Büyüklüğü;Katılımcı;E-posta", lines[0]);
        Assert.StartsWith("Arena;2;Ada Yılmaz;", lines[1]);
        Assert.Contains(";100;C#;SQLite", lines[1]);
        Assert.Contains(";30;Python;Redis", lines[2]);
        Assert.StartsWith(SettlementCsv.Unassigned + ";;", lines[3]);
    }

    [Fact]
    public void Csv_QuotesSpecialCharactersAndNeutralizesFormulas()
    {
        var csv = SettlementCsv.Write(Details());

        Assert.Contains(";\"Bob; \"\"Jr\"\"\";", csv);
        Assert.Contains(";'=Cem;", csv);
    }

    [Fact]
    public void Csv_BeforeRouting_HasOnlyHeader()
    {
        var csv = SettlementCsv.Write(Details(settled: false));

        Assert.Single(csv.Split("\r\n", StringSplitOptions.RemoveEmptyEntries));
    }

    [Fact]
    public void Csv_Bytes_StartWithUtf8Bom()
    {
        var bytes = SettlementCsv.WriteUtf8WithBom(Details());

        Assert.Equal(new byte[] { 0xEF, 0xBB, 0xBF }, bytes.Take(3));
        Assert.StartsWith("Proje;", Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3));
    }

    [Fact]
    public void Html_ContainsSummaryTeamsAndLists()
    {
        var html = SettlementHtml.Write(Details());

        Assert.StartsWith("<!DOCTYPE html>", html);
        Assert.Contains("AI &lt;Cup&gt;", html);
        Assert.DoesNotContain("<Cup>", html);
        Assert.Contains("Dönem 2026-27", html);
        Assert.Contains("3 katılımcı", html);
        Assert.Contains("1 takım / 2 proje", html);
        Assert.Contains("1 yerleştirilemeyen", html);
        Assert.Contains("ortalama uyum %65", html);
        Assert.Contains("Ada Yılmaz", html);
        Assert.Contains("%100", html);
        Assert.Contains("%30", html);
        Assert.Contains("Yerleştirilemeyen katılımcılar", html);
        Assert.Contains("Cem", html);
        Assert.Contains("Açılmayan projeler", html);
        Assert.Contains("Büyük Proje (en az 5 kişi)", html);
    }

    [Fact]
    public void Html_OneTableRowPerTeam()
    {
        var html = SettlementHtml.Write(Details());

        Assert.Equal(1, CountOccurrences(html, "vertical-align:top;"));
    }

    [Fact]
    public void Html_BeforeRouting_ShowsPlaceholder()
    {
        var html = SettlementHtml.Write(Details(settled: false));

        Assert.Contains("Henüz dağıtım yapılmadı.", html);
        Assert.DoesNotContain("Dağıtım:", html);
        Assert.DoesNotContain("ortalama uyum", html);
        Assert.DoesNotContain("Açılmayan projeler", html);
    }

    [Fact]
    public void Writers_RejectNull()
    {
        Assert.Throws<ArgumentNullException>(() => SettlementCsv.Write(null!));
        Assert.Throws<ArgumentNullException>(() => SettlementHtml.Write(null!));
    }

    private static int CountOccurrences(string text, string value)
    {
        var count = 0;
        for (var index = text.IndexOf(value, StringComparison.Ordinal); index >= 0; index = text.IndexOf(value, index + value.Length, StringComparison.Ordinal))
            count++;

        return count;
    }
}
