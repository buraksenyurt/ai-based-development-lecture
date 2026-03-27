using BookApp.Domain;

namespace BookApp.Domain.Tests;

public class BookTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesBook()
    {
        var bookId = Guid.NewGuid();
        var book = new Book(bookId, "Clean Code", "Robert C. Martin", 39.99m, 464);

        Assert.Equal(bookId, book.BookId);
        Assert.Equal("Clean Code", book.Title);
        Assert.Equal("Robert C. Martin", book.Author);
        Assert.Equal(39.99m, book.Price);
        Assert.Equal(464, book.PageCount);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptyTitle_ThrowsArgumentException(string? invalidTitle)
    {
        Assert.Throws<ArgumentException>(() =>
            new Book(Guid.NewGuid(), invalidTitle!, "Author", 10.00m, 100));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptyAuthor_ThrowsArgumentException(string? invalidAuthor)
    {
        Assert.Throws<ArgumentException>(() =>
            new Book(Guid.NewGuid(), "Title", invalidAuthor!, 10.00m, 100));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_WithNonPositivePrice_ThrowsArgumentException(decimal invalidPrice)
    {
        Assert.Throws<ArgumentException>(() =>
            new Book(Guid.NewGuid(), "Title", "Author", invalidPrice, 100));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-50)]
    public void Constructor_WithNonPositivePageCount_ThrowsArgumentException(int invalidPageCount)
    {
        Assert.Throws<ArgumentException>(() =>
            new Book(Guid.NewGuid(), "Title", "Author", 10.00m, invalidPageCount));
    }

    [Fact]
    public void UpdatePrice_WithValidPrice_UpdatesPrice()
    {
        var book = new Book(Guid.NewGuid(), "Title", "Author", 10.00m, 100);
        book.UpdatePrice(25.00m);

        Assert.Equal(25.00m, book.Price);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void UpdatePrice_WithNonPositivePrice_ThrowsArgumentException(decimal invalidPrice)
    {
        var book = new Book(Guid.NewGuid(), "Title", "Author", 10.00m, 100);

        Assert.Throws<ArgumentException>(() => book.UpdatePrice(invalidPrice));
    }
}
