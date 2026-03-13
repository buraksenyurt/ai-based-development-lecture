using DeppoApp.Data;
using DeppoApp.Domain;
using Moq;

namespace DeppoApp.Application.Tests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly ProductService _sut;

    public ProductServiceTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _sut = new ProductService(_repositoryMock.Object);
    }

    [Fact]
    public void CreateProduct_WithValidParameters_ShouldReturnProductId()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _repositoryMock
            .Setup(r => r.Save(It.IsAny<Product>()))
            .Returns(productId);

        // Act
        var result = _sut.CreateProduct(productId, "Laptop", 999.99M, 10);

        // Assert
        Assert.Equal(productId, result);
    }

    [Fact]
    public void CreateProduct_WithValidParameters_ShouldCallSaveExactlyOnce()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _repositoryMock
            .Setup(r => r.Save(It.IsAny<Product>()))
            .Returns(productId);

        // Act
        _sut.CreateProduct(productId, "Laptop", 999.99M, 10);

        // Assert
        _repositoryMock.Verify(r => r.Save(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public void CreateProduct_WithValidParameters_ShouldSaveProductWithCorrectProperties()
    {
        // Arrange
        var productId = Guid.NewGuid();
        const string title = "Laptop";
        const decimal unitPrice = 999.99M;
        const int stockCount = 10;

        Product? capturedProduct = null;
        _repositoryMock
            .Setup(r => r.Save(It.IsAny<Product>()))
            .Callback<Product>(p => capturedProduct = p)
            .Returns(productId);

        // Act
        _sut.CreateProduct(productId, title, unitPrice, stockCount);

        // Assert
        Assert.NotNull(capturedProduct);
        Assert.Equal(productId, capturedProduct.ProductId);
        Assert.Equal(title, capturedProduct.Title);
        Assert.Equal(unitPrice, capturedProduct.UnitPrice);
        Assert.Equal(stockCount, capturedProduct.StockCount);
    }

    [Fact]
    public void CreateProduct_WithZeroUnitPrice_ShouldThrowArgumentException()
    {
        // Arrange
        var productId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => _sut.CreateProduct(productId, "Laptop", 0M, 10));

        _repositoryMock.Verify(r => r.Save(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public void CreateProduct_WithNegativeUnitPrice_ShouldThrowArgumentException()
    {
        // Arrange
        var productId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => _sut.CreateProduct(productId, "Laptop", -50M, 10));

        _repositoryMock.Verify(r => r.Save(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public void CreateProduct_WithNegativeStockCount_ShouldThrowArgumentException()
    {
        // Arrange
        var productId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => _sut.CreateProduct(productId, "Laptop", 999.99M, -1));

        _repositoryMock.Verify(r => r.Save(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public void CreateProduct_WithZeroStockCount_ShouldSaveProductWithZeroStock()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _repositoryMock
            .Setup(r => r.Save(It.IsAny<Product>()))
            .Returns(productId);

        // Act
        var result = _sut.CreateProduct(productId, "Laptop", 999.99M, 0);

        // Assert
        Assert.Equal(productId, result);
        _repositoryMock.Verify(r => r.Save(It.Is<Product>(p => p.StockCount == 0)), Times.Once);
    }
}
