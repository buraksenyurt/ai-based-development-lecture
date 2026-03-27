namespace BookApp.Domain;

public class Book
{
    public Guid BookId { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public decimal Price { get; private set; }
    public int PageCount { get; private set; }

    public Book(Guid bookId, string title, string author, decimal price, int pageCount)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));

        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Author cannot be empty.", nameof(author));

        if (price <= 0)
            throw new ArgumentException("Price must be greater than zero.", nameof(price));

        if (pageCount <= 0)
            throw new ArgumentException("Page count must be greater than zero.", nameof(pageCount));

        BookId = bookId;
        Title = title;
        Author = author;
        Price = price;
        PageCount = pageCount;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("New price must be greater than zero.", nameof(newPrice));

        Price = newPrice;
    }
}
