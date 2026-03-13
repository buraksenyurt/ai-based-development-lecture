using DeppoApp.Domain;

namespace DeppoApp.Data;

public interface IProductRepository
{
    Guid Save(Product product);
}
