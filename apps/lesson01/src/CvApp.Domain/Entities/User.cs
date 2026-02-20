namespace CvApp.Domain.Entities;

/// <summary>
/// Sisteme kayıt olan kullanıcıyı temsil eder.
/// </summary>
public class User
{
    public Guid UserId { get; private set; }

    /// <summary>
    /// Kullanıcı adı ve soyadından oluşur. Min 10, Max 100 karakter.
    /// </summary>
    public string Fullname { get; private set; } = string.Empty;

    private User() { }

    public static User Create(string fullname)
    {
        ValidateFullname(fullname);

        return new User
        {
            UserId = Guid.NewGuid(),
            Fullname = fullname
        };
    }

    public void UpdateFullname(string fullname)
    {
        ValidateFullname(fullname);
        Fullname = fullname;
    }

    private static void ValidateFullname(string fullname)
    {
        if (string.IsNullOrWhiteSpace(fullname))
            throw new ArgumentException("Fullname boş olamaz.", nameof(fullname));

        if (fullname.Length < 10)
            throw new ArgumentException("Fullname en az 10 karakter olmalıdır.", nameof(fullname));

        if (fullname.Length > 100)
            throw new ArgumentException("Fullname en fazla 100 karakter olabilir.", nameof(fullname));
    }
}
