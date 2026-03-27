using BookApp.Data;
using BookApp.Domain;

namespace BookApp.Application;

public class BookService(IBookRepository bookRepository)
{
    public Guid AddBook(Guid bookId, string title, string author, decimal price, int pageCount)
    {
        var book = new Book(bookId, title, author, price, pageCount);
        return bookRepository.Save(book);
    }

    public Guid UpdateBookPrice(Guid bookId, string title, string author, decimal currentPrice, int pageCount, decimal newPrice)
    {
        var book = new Book(bookId, title, author, currentPrice, pageCount);
        book.UpdatePrice(newPrice);
        return bookRepository.Save(book);
    }
}
