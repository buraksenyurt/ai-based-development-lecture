using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using ProjectRouter.Application.Services;

namespace ProjectRouter.Application.Reports;

/// <summary>
/// A compact, e-mail friendly HTML summary of a settlement: one table row per team with its members listed inline,
/// so even 25+ teams stay on a couple of screens. Only inline styles and tables are used, because most e-mail clients
/// ignore &lt;style&gt; blocks, external CSS and modern layout (flex/grid).
/// </summary>
public static class SettlementHtml
{
    private const string Border = "#d0d7de";
    private const string ColorDivPrefix = "<div style=\"color:";
    private const string CloseDiv = "</div>";

    // Keeps Turkish characters readable in the source (the document is UTF-8) while still escaping markup.
    private static readonly HtmlEncoder Encoder = HtmlEncoder.Create(UnicodeRanges.All);
    private const string Muted = "#57606a";

    public static string Write(CompetitionDetails details)
    {
        ArgumentNullException.ThrowIfNull(details);

        var competition = details.Competition;
        var html = new StringBuilder();

        html.Append("<!DOCTYPE html><html lang=\"tr\"><head><meta charset=\"utf-8\"><title>")
            .Append(E(competition.Title)).Append(" - Dağıtım Sonucu</title></head>")
            .Append("<body style=\"margin:0;padding:16px;background:#f6f8fa;font-family:'Segoe UI',Arial,sans-serif;font-size:14px;color:#1f2328;\">")
            .Append("<table role=\"presentation\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"max-width:960px;margin:0 auto;background:#ffffff;border:1px solid ")
            .Append(Border).Append(";border-collapse:collapse;\">");

        AppendHeader(html, details);
        AppendTeams(html, details);
        AppendFooterLists(html, details);

        html.Append("</table></body></html>");
        return html.ToString();
    }

    private static void AppendHeader(StringBuilder html, CompetitionDetails details)
    {
        var competition = details.Competition;
        var placedScores = details.Teams.SelectMany(t => t.Members).Select(m => m.Score).ToList();

        var subtitle = $"Dönem {competition.Session}";
        if (competition.SettledAt is { } settledAt)
            subtitle += " · Dağıtım: " + settledAt.ToLocalTime().ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);

        var stats = new List<string>
        {
            $"{details.Participants.Count} katılımcı",
            $"{details.Teams.Count} takım / {details.Projects.Count} proje",
            $"{details.UnassignedParticipants.Count} yerleştirilemeyen",
        };
        if (placedScores.Count > 0)
            stats.Add($"ortalama uyum %{Percent(placedScores.Average())}");

        html.Append("<tr><td style=\"padding:16px 20px;border-bottom:1px solid ").Append(Border).Append(";\">")
            .Append("<div style=\"font-size:20px;font-weight:600;\">").Append(E(competition.Title)).Append(CloseDiv)
            .Append(ColorDivPrefix).Append(Muted).Append(";margin-top:2px;\">").Append(E(subtitle)).Append(CloseDiv)
            .Append("<div style=\"margin-top:8px;\">").Append(E(string.Join(" · ", stats))).Append(CloseDiv)
            .Append(ColorDivPrefix).Append(Muted).Append(";font-size:12px;margin-top:6px;\">Uyum: ")
            .Append(Legend(MatchLevel.Strong, "güçlü (≥ %75)")).Append(" · ")
            .Append(Legend(MatchLevel.Partial, "kısmi")).Append(" · ")
            .Append(Legend(MatchLevel.None, "eşleşme yok"))
            .Append(CloseDiv).Append("</td></tr>");
    }

    private static void AppendTeams(StringBuilder html, CompetitionDetails details)
    {
        if (details.Teams.Count == 0)
        {
            html.Append("<tr><td style=\"padding:16px 20px;color:").Append(Muted).Append(";\">Henüz dağıtım yapılmadı.</td></tr>");
            return;
        }

        html.Append("<tr><td style=\"padding:0;\"><table role=\"presentation\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:collapse;\">")
            .Append("<tr style=\"background:#f6f8fa;text-align:left;font-size:12px;color:").Append(Muted).Append(";\">")
            .Append(Th("Proje", "32%")).Append(Th("Kişi", "8%")).Append(Th("Takım", "60%"))
            .Append("</tr>");

        for (var i = 0; i < details.Teams.Count; i++)
        {
            var team = details.Teams[i];
            var project = team.Project;
            var background = i % 2 == 0 ? "#ffffff" : "#fbfcfd";
            var tech = string.Join(", ", project.TechStack.Languages) + " · " + string.Join(", ", project.TechStack.Databases);
            var members = string.Join(", ", team.Members.Select(Member));

            html.Append("<tr style=\"background:").Append(background).Append(";vertical-align:top;\">")
                .Append(Td()).Append("<div style=\"font-weight:600;\">").Append(E(project.Title)).Append("</div>")
                .Append(ColorDivPrefix).Append(Muted).Append(";font-size:12px;\">").Append(E(tech)).Append(CloseDiv).Append("</td>")
                .Append(Td()).Append(team.Members.Count.ToString(CultureInfo.InvariantCulture))
                .Append(ColorDivPrefix).Append(Muted).Append(";font-size:12px;\">")
                .Append(E($"{project.Team.Min}–{project.Team.Max}")).Append(CloseDiv).Append("</td>")
                .Append(Td()).Append(members).Append("</td>")
                .Append("</tr>");
        }

        html.Append("</table></td></tr>");
    }

    private static void AppendFooterLists(StringBuilder html, CompetitionDetails details)
    {
        if (details.UnassignedParticipants.Count > 0)
        {
            AppendList(html, "Yerleştirilemeyen katılımcılar",
                string.Join(", ", details.UnassignedParticipants.Select(p => E(p.FullName))));
        }

        if (details.ClosedProjects.Count > 0)
        {
            AppendList(html, "Açılmayan projeler",
                string.Join(", ", details.ClosedProjects.Select(p => E($"{p.Title} (en az {p.Team.Min} kişi)"))));
        }
    }

    private static void AppendList(StringBuilder html, string title, string encodedItems) =>
        html.Append("<tr><td style=\"padding:12px 20px;border-top:1px solid ").Append(Border).Append(";\">")
            .Append("<span style=\"font-weight:600;\">").Append(E(title)).Append(":</span> ")
            .Append(encodedItems)
            .Append("</td></tr>");

    private static string Member(TeamMember member) =>
        $"<span style=\"white-space:nowrap;\">{E(member.Participant.FullName)} " +
        $"<span style=\"color:{Color(MatchLevels.Classify(member.Score))};font-size:12px;font-weight:600;\">%{Percent(member.Score)}</span></span>";

    private static string Legend(MatchLevel level, string text) =>
        $"<span style=\"color:{Color(level)};font-weight:600;\">■</span> {E(text)}";

    private static string Color(MatchLevel level) => level switch
    {
        MatchLevel.Strong => "#1a7f37",
        MatchLevel.Partial => "#9a6700",
        _ => "#cf222e",
    };

    private static string Th(string text, string width) =>
        $"<th style=\"padding:8px 12px;width:{width};font-weight:600;border-bottom:1px solid {Border};\">{E(text)}</th>";

    private static string Td() => $"<td style=\"padding:8px 12px;border-bottom:1px solid {Border};\">";

    private static string Percent(double score) => Math.Round(score * 100).ToString(CultureInfo.InvariantCulture);

    private static string E(string value) => Encoder.Encode(value);
}
