using BookApp.Domain;

namespace BookApp.Data;

public interface IBookRepository
{
    Guid Save(Book book);
}
