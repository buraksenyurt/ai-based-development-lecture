using DeppoApp.Data;
using DeppoApp.Domain;

namespace DeppoApp.Application;

public class ProductService(IProductRepository productRepository)
{

    // CreateProduct için bir entegrasyon testi nasıl yazılır?
    public Guid CreateProduct(Guid productId, string productName, decimal price, int stockCount)
    {
        var product = new Product(productId, productName, price);
        product.IncreaseStock(stockCount);
        var createdId = productRepository.Save(product);
        return createdId;
    }
}
