using System.Globalization;
using System.Text;
using ProjectRouter.Application.Services;
using ProjectRouter.Domain;

namespace ProjectRouter.Application.Reports;

/// <summary>
/// Settlement as a spreadsheet-friendly CSV: one row per participant, placed participants first (grouped by project),
/// then the participants that could not be placed.
/// Uses ';' as separator so that Excel with Turkish regional settings opens it in columns.
/// </summary>
public static class SettlementCsv
{
    public const char Separator = ';';
    public const string Unassigned = "(Yerleştirilemedi)";

    private static readonly string[] Header =
    [
        "Proje", "Takım Büyüklüğü", "Katılımcı", "E-posta", "Okul", "Bölüm", "Sınıf", "GitHub",
        "Uyum (%)", "Dil Tercihleri", "Veritabanı Tercihleri",
    ];

    /// <summary>The CSV as UTF-8 bytes with a BOM, so Excel detects the Turkish characters correctly.</summary>
    public static byte[] WriteUtf8WithBom(CompetitionDetails details)
    {
        var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
        return [.. encoding.GetPreamble(), .. encoding.GetBytes(Write(details))];
    }

    public static string Write(CompetitionDetails details)
    {
        ArgumentNullException.ThrowIfNull(details);

        var csv = new StringBuilder();
        AppendRow(csv, Header);

        foreach (var team in details.Teams)
        {
            foreach (var member in team.Members)
                AppendRow(csv, Row(team.Project.Title, team.Members.Count.ToString(CultureInfo.InvariantCulture), member.Participant, Percent(member.Score)));
        }

        foreach (var participant in details.UnassignedParticipants)
            AppendRow(csv, Row(Unassigned, "", participant, ""));

        return csv.ToString();
    }

    private static string Percent(double score) =>
        Math.Round(score * 100).ToString(CultureInfo.InvariantCulture);

    private static string[] Row(string project, string teamSize, Participant participant, string score) =>
    [
        project,
        teamSize,
        participant.FullName,
        participant.Email,
        participant.University,
        participant.Department,
        participant.Class.ToString(CultureInfo.InvariantCulture),
        participant.GithubUrl ?? "",
        score,
        string.Join(" > ", participant.Languages),
        string.Join(" > ", participant.Databases),
    ];

    private static void AppendRow(StringBuilder csv, IEnumerable<string> fields)
    {
        csv.AppendJoin(Separator, fields.Select(Escape));
        csv.Append("\r\n");
    }

    /// <summary>
    /// Quotes fields that contain the separator, quotes or line breaks (RFC 4180) and neutralizes values that a
    /// spreadsheet would treat as a formula (CSV injection: =, +, -, @ at the start).
    /// </summary>
    private static string Escape(string value)
    {
        if (value.Length > 0 && value[0] is '=' or '+' or '-' or '@' or '\t' or '\r')
            value = "'" + value;

        if (value.IndexOfAny([Separator, '"', '\r', '\n']) < 0)
            return value;

        return "\"" + value.Replace("\"", "\"\"") + "\"";
    }
}
