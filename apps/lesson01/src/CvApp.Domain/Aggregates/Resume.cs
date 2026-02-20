using CvApp.Domain.Entities;

namespace CvApp.Domain.Aggregates;

/// <summary>
/// CV'nin detaylarını taşıyan Aggregate Root.
/// User, Contact ve Skills gibi bilgileri kapsar.
/// </summary>
public class Resume
{
    public Guid ResumeId { get; private set; }

    public User User { get; private set; } = null!;

    private readonly List<Contact> _contacts = new();
    public IReadOnlyCollection<Contact> Contacts => _contacts.AsReadOnly();

    private Resume() { }

    public static Resume Create(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return new Resume
        {
            ResumeId = Guid.NewGuid(),
            User = user
        };
    }

    public void AddContact(Contact contact)
    {
        ArgumentNullException.ThrowIfNull(contact);

        if (contact.RelatedUser != User.UserId)
            throw new InvalidOperationException("İletişim bilgisi bu resume'a bağlı kullanıcıya ait değil.");

        _contacts.Add(contact);
    }

    public void RemoveContact(Guid contactId)
    {
        var contact = _contacts.FirstOrDefault(c => c.ContactId == contactId)
            ?? throw new InvalidOperationException($"ContactId {contactId} bu resume'da bulunamadı.");

        _contacts.Remove(contact);
    }
}
