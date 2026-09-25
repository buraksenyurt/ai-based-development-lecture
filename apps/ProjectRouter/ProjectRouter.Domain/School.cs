namespace ProjectRouter.Domain;

/// <summary>Where the participant studies ("Okul", "Branş", "Sınıf" in docs/project-router.md).</summary>
public sealed record School
{
    public string University { get; }
    public string Department { get; }
    public int Class { get; }

    public School(string university, string department, int @class)
    {
        if (@class <= 0)
            throw new DomainRuleException(DomainRuleException.Validation, "Class must be greater than zero.");

        University = Guard.Required(university, "University");
        Department = Guard.Required(department, "Department");
        Class = @class;
    }
}
