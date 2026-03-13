using Dapper;
using DeppoApp.Domain;
using Npgsql;

namespace DeppoApp.Data;

public class ProductRepository
    : IProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Save metodunun beklediğimiz şekilde çalıştığını nasıl test ederiz?
    public Guid Save(Product product)
    {
        const string sql = """
            INSERT INTO products (product_id, title, unit_price, stock_count)
            VALUES (@ProductId, @Title, @UnitPrice, @StockCount)
            ON CONFLICT (product_id) DO UPDATE
                SET title       = EXCLUDED.title,
                    unit_price  = EXCLUDED.unit_price,
                    stock_count = EXCLUDED.stock_count
            RETURNING product_id;
            """;

        using var connection = new NpgsqlConnection(_connectionString);
        return connection.ExecuteScalar<Guid>(sql, new
        {
            product.ProductId,
            product.Title,
            product.UnitPrice,
            product.StockCount
        });
    }
}
