using CvApp.Domain.Aggregates;
using CvApp.Domain.Entities;
using CvApp.Domain.Interfaces;
using CvApp.Infrastructure.Persistence.Documents;
using CvApp.Infrastructure.Persistence.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CvApp.Infrastructure.Persistence.Repositories;

public class ResumeRepository : IResumeRepository
{
    private readonly IMongoCollection<ResumeDocument> _collection;

    public ResumeRepository(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _collection = database.GetCollection<ResumeDocument>(settings.Value.ResumesCollectionName);
    }

    public async Task<Resume?> GetByIdAsync(Guid resumeId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ResumeDocument>.Filter.Eq(r => r.ResumeId, resumeId);
        var document = await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return document is null ? null : MapToDomain(document);
    }

    public async Task<IEnumerable<Resume>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ResumeDocument>.Filter.Eq(r => r.User.UserId, userId);
        var documents = await _collection.Find(filter).ToListAsync(cancellationToken);
        return documents.Select(MapToDomain);
    }

    public async Task<IEnumerable<Resume>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var documents = await _collection.Find(_ => true).ToListAsync(cancellationToken);
        return documents.Select(MapToDomain);
    }

    public async Task AddAsync(Resume resume, CancellationToken cancellationToken = default)
    {
        var document = MapToDocument(resume);
        await _collection.InsertOneAsync(document, cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(Resume resume, CancellationToken cancellationToken = default)
    {
        var document = MapToDocument(resume);
        var filter = Builders<ResumeDocument>.Filter.Eq(r => r.ResumeId, resume.ResumeId);
        await _collection.ReplaceOneAsync(filter, document, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(Guid resumeId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ResumeDocument>.Filter.Eq(r => r.ResumeId, resumeId);
        await _collection.DeleteOneAsync(filter, cancellationToken);
    }

    // ── Mapping Helpers ─────────────────────────────────────────────────────────

    private static ResumeDocument MapToDocument(Resume resume)
    {
        return new ResumeDocument
        {
            ResumeId = resume.ResumeId,
            User = new UserDocument
            {
                UserId = resume.User.UserId,
                Fullname = resume.User.Fullname
            },
            Contacts = resume.Contacts.Select(c => new ContactDocument
            {
                ContactId = c.ContactId,
                Kind = c.Kind,
                RelatedUser = c.RelatedUser,
                Value = c.Value
            }).ToList()
        };
    }

    private static Resume MapToDomain(ResumeDocument document)
    {
        // Reflection ile private constructor ve setter'ları bypass ederek domain nesnesi oluşturulur.
        var user = CreateUserFromDocument(document.User);
        var resume = CreateResumeFromDocument(document.ResumeId, user);

        foreach (var contactDoc in document.Contacts)
        {
            var contact = Contact.Create(contactDoc.Kind, contactDoc.RelatedUser, contactDoc.Value);
            SetGuid(contact, nameof(Contact.ContactId), contactDoc.ContactId);
            resume.AddContact(contact);
        }

        return resume;
    }

    private static User CreateUserFromDocument(UserDocument doc)
    {
        var user = User.Create(doc.Fullname);
        SetGuid(user, nameof(User.UserId), doc.UserId);
        return user;
    }

    private static Resume CreateResumeFromDocument(Guid resumeId, User user)
    {
        var resume = Resume.Create(user);
        SetGuid(resume, nameof(Resume.ResumeId), resumeId);
        return resume;
    }

    private static void SetGuid(object obj, string propertyName, Guid value)
    {
        var property = obj.GetType().GetProperty(propertyName)
            ?? throw new InvalidOperationException($"Property '{propertyName}' bulunamadı.");

        property.SetValue(obj, value);
    }
}
