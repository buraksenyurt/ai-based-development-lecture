using System.Text;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectRouter.Domain;
using CompetitionDetailsModel = ProjectRouter.Web.Pages.Competitions.DetailsModel;

namespace ProjectRouter.Web.Tests.Pages;

public class CompetitionExportTests
{
    private readonly PageTestContext _context = new();
    private readonly Participant _ali = PageTestContext.Participant("Ali");
    private readonly Participant _veli = PageTestContext.Participant("Veli");
    private readonly ProjectIdea _arena = PageTestContext.Project("Arena", min: 1, max: 3);

    private CompetitionDetailsModel DetailsPage() => PageTestContext.Attach(new CompetitionDetailsModel(_context.CompetitionService));

    private Competition ArrangeSettledCompetition()
    {
        var competition = new Competition(Guid.NewGuid(), "AI Cup", "2026-27", [_arena.Id], [_ali.Id, _veli.Id]);
        competition.AssignSettlement(
            new Dictionary<Guid, IReadOnlyList<Guid>> { [_arena.Id] = [_ali.Id, _veli.Id] }, [_arena], DateTime.UtcNow);

        _context.Competitions.Setup(r => r.GetById(competition.Id)).Returns(competition);
        _context.Participants.Setup(r => r.GetByIds(It.IsAny<IEnumerable<Guid>>())).Returns([_ali, _veli]);
        _context.Projects.Setup(r => r.GetByIds(It.IsAny<IEnumerable<Guid>>())).Returns([_arena]);
        return competition;
    }

    [Fact]
    public void OnGetExportCsv_ReturnsCsvFileWithBom()
    {
        var competition = ArrangeSettledCompetition();

        var result = DetailsPage().OnGetExportCsv(competition.Id);

        var file = Assert.IsType<FileContentResult>(result);
        Assert.Equal("text/csv; charset=utf-8", file.ContentType);
        Assert.Equal($"dagitim-{competition.Id}.csv", file.FileDownloadName);
        Assert.Equal(new byte[] { 0xEF, 0xBB, 0xBF }, file.FileContents.Take(3));

        var lines = Encoding.UTF8.GetString(file.FileContents, 3, file.FileContents.Length - 3)
            .Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(3, lines.Length);
        Assert.StartsWith("Arena;2;Ali;", lines[1]);
        Assert.StartsWith("Arena;2;Veli;", lines[2]);
    }

    [Fact]
    public void OnGetExportCsv_Unknown_ReturnsNotFound()
    {
        Assert.IsType<NotFoundResult>(DetailsPage().OnGetExportCsv(Guid.NewGuid()));
    }

    [Fact]
    public void OnGetExportHtml_ReturnsHtmlContent()
    {
        var competition = ArrangeSettledCompetition();

        var result = DetailsPage().OnGetExportHtml(competition.Id);

        var content = Assert.IsType<ContentResult>(result);
        Assert.Equal("text/html; charset=utf-8", content.ContentType);
        Assert.Contains("AI Cup", content.Content);
        Assert.Contains("Arena", content.Content);
        Assert.Contains("Veli", content.Content);
    }

    [Fact]
    public void OnGetExportHtml_Unknown_ReturnsNotFound()
    {
        Assert.IsType<NotFoundResult>(DetailsPage().OnGetExportHtml(Guid.NewGuid()));
    }
}
