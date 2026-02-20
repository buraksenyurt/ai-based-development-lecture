namespace CvApp.Infrastructure.Persistence.Settings;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string ResumesCollectionName { get; set; } = "resumes";
}
