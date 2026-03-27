using BookApp.Application;
using BookApp.Data;
using BookApp.Domain;
using Moq;

namespace BookApp.Application.Tests;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _repositoryMock;
    private readonly BookService _bookService;

    public BookServiceTests()
    {
        _repositoryMock = new Mock<IBookRepository>();
        _bookService = new BookService(_repositoryMock.Object);
    }

    [Fact]
    public void AddBook_WithValidParameters_ReturnsBookId()
    {
        var bookId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.Save(It.IsAny<Book>())).Returns(bookId);

        var result = _bookService.AddBook(bookId, "Clean Code", "Robert C. Martin", 39.99m, 464);

        Assert.Equal(bookId, result);
    }

    [Fact]
    public void AddBook_WithValidParameters_CallsRepositorySaveOnce()
    {
        var bookId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.Save(It.IsAny<Book>())).Returns(bookId);

        _bookService.AddBook(bookId, "Clean Code", "Robert C. Martin", 39.99m, 464);

        _repositoryMock.Verify(r => r.Save(It.IsAny<Book>()), Times.Once);
    }

    [Fact]
    public void AddBook_WithValidParameters_SavesCorrectBookData()
    {
        var bookId = Guid.NewGuid();
        Book? savedBook = null;
        _repositoryMock
            .Setup(r => r.Save(It.IsAny<Book>()))
            .Callback<Book>(b => savedBook = b)
            .Returns(bookId);

        _bookService.AddBook(bookId, "Clean Code", "Robert C. Martin", 39.99m, 464);

        Assert.NotNull(savedBook);
        Assert.Equal(bookId, savedBook.BookId);
        Assert.Equal("Clean Code", savedBook.Title);
        Assert.Equal("Robert C. Martin", savedBook.Author);
        Assert.Equal(39.99m, savedBook.Price);
        Assert.Equal(464, savedBook.PageCount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AddBook_WithNonPositivePrice_ThrowsArgumentException(decimal invalidPrice)
    {
        Assert.Throws<ArgumentException>(() =>
            _bookService.AddBook(Guid.NewGuid(), "Title", "Author", invalidPrice, 100));
    }

    [Fact]
    public void AddBook_WithZeroPageCount_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            _bookService.AddBook(Guid.NewGuid(), "Title", "Author", 10.00m, 0));
    }

    [Fact]
    public void UpdateBookPrice_WithValidNewPrice_SavesUpdatedBook()
    {
        var bookId = Guid.NewGuid();
        Book? savedBook = null;
        _repositoryMock
            .Setup(r => r.Save(It.IsAny<Book>()))
            .Callback<Book>(b => savedBook = b)
            .Returns(bookId);

        _bookService.UpdateBookPrice(bookId, "Title", "Author", 10.00m, 100, 25.00m);

        Assert.NotNull(savedBook);
        Assert.Equal(25.00m, savedBook.Price);
    }

    [Fact]
    public void UpdateBookPrice_WithNonPositiveNewPrice_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            _bookService.UpdateBookPrice(Guid.NewGuid(), "Title", "Author", 10.00m, 100, 0));
    }
}
