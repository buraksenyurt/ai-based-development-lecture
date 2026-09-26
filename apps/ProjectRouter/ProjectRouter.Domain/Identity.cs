namespace ProjectRouter.Domain;

/// <summary>Who the participant is ("Kimlik" in docs/project-router.md).</summary>
public sealed record Identity
{
    public string FullName { get; }
    public string Email { get; }

    public Identity(string fullName, string email)
    {
        var mail = Guard.Required(email, "Email");
        if (!mail.Contains('@'))
            throw new DomainRuleException(DomainRuleException.Validation, "Email is not valid.");

        FullName = Guard.Required(fullName, "Full name");
        Email = mail;
    }
}
