using CvApp.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace CvApp.Infrastructure.Persistence.Documents;

public class ResumeDocument
{
    [BsonId]
    public Guid ResumeId { get; set; }

    public UserDocument User { get; set; } = null!;

    public List<ContactDocument> Contacts { get; set; } = new();
}

public class UserDocument
{
    public Guid UserId { get; set; }
    public string Fullname { get; set; } = string.Empty;
}

public class ContactDocument
{
    public Guid ContactId { get; set; }
    public ContactType Kind { get; set; }
    public Guid RelatedUser { get; set; }
    public string Value { get; set; } = string.Empty;
}
