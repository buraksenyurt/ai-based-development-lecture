using BookApp.Domain;
using Dapper;
using Npgsql;

namespace BookApp.Data;

public class BookRepository(string connectionString) : IBookRepository
{
    public Guid Save(Book book)
    {
        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        var sql = """
            INSERT INTO books (book_id, title, author, price, page_count)
            VALUES (@BookId, @Title, @Author, @Price, @PageCount)
            ON CONFLICT (book_id) DO UPDATE
            SET title = EXCLUDED.title,
                author = EXCLUDED.author,
                price = EXCLUDED.price,
                page_count = EXCLUDED.page_count;
            """;

        connection.Execute(sql, new
        {
            book.BookId,
            book.Title,
            book.Author,
            book.Price,
            book.PageCount
        });

        return book.BookId;
    }
}
