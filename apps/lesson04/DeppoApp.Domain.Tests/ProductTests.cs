namespace DeppoApp.Domain.Tests;

public class ProductTests
{
    // ──────────────────────────────────────────────
    // Constructor
    // ──────────────────────────────────────────────

    [Fact]
    public void Constructor_WithValidParameters_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        string title = "AyBiEm 13 inch i7 1 TB 64 Gb laptop";
        decimal unitPrice = 1499.99M;

        // Act
        var product = new Product(id, title, unitPrice);

        // Assert
        Assert.Equal(id, product.ProductId);
        Assert.Equal(title, product.Title);
        Assert.Equal(unitPrice, product.UnitPrice);
    }

    [Fact]
    public void Constructor_InitialStockCount_ShouldBeZero()
    {
        // Arrange & Act
        var product = new Product(Guid.NewGuid(), "Test Product", 99.99M);

        // Assert
        Assert.Equal(0, product.StockCount);
    }

    [Fact]
    public void Constructor_WhenUnitPriceIsNegative_ShouldThrowArgumentException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        string title = "AyBiEm 13 inch i7 1 TB 64 Gb laptop";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Product(productId, title, -1499.99M));
    }

    [Fact]
    public void Constructor_WhenUnitPriceIsZero_ShouldThrowArgumentException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        string title = "AyBiEm 13 inch i7 1 TB 64 Gb laptop";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Product(productId, title, 0M));
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-100)]
    [InlineData(-9999.99)]
    public void Constructor_WhenUnitPriceIsLessThanOrEqualToZero_ShouldThrowArgumentException(decimal invalidPrice)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Product(Guid.NewGuid(), "Test Product", invalidPrice));
    }

    // ──────────────────────────────────────────────
    // IncreaseStock
    // ──────────────────────────────────────────────

    [Fact]
    public void IncreaseStock_ShouldIncreaseStockCount()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "AyBiEm 13 inch i7 1 TB 64 Gb laptop", 1499.99M);
        int initialStock = product.StockCount;
        int increaseAmount = 5;

        // Act
        product.IncreaseStock(increaseAmount);

        // Assert
        Assert.Equal(initialStock + increaseAmount, product.StockCount);
    }

    [Fact]
    public void IncreaseStock_ByOne_ShouldIncrementStockByOne()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "Test Product", 50M);

        // Act
        product.IncreaseStock(1);

        // Assert
        Assert.Equal(1, product.StockCount);
    }

    [Fact]
    public void IncreaseStock_CalledMultipleTimes_ShouldAccumulateStock()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "Test Product", 50M);

        // Act
        product.IncreaseStock(3);
        product.IncreaseStock(7);
        product.IncreaseStock(10);

        // Assert
        Assert.Equal(20, product.StockCount);
    }

    [Fact]
    public void IncreaseStock_ByZero_ShouldNotChangeStockCount()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "Test Product", 50M);
        product.IncreaseStock(10);
        int stockBefore = product.StockCount;

        // Act
        product.IncreaseStock(0);

        // Assert
        Assert.Equal(stockBefore, product.StockCount);
    }

    [Fact]
    public void IncreaseStock_WithNegativeAmount_ShouldThrowArgumentException()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "Test Product", 50M);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => product.IncreaseStock(-1));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(1000)]
    public void IncreaseStock_WithVariousAmounts_ShouldSetCorrectStockCount(int amount)
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "Test Product", 50M);

        // Act
        product.IncreaseStock(amount);

        // Assert
        Assert.Equal(amount, product.StockCount);
    }

    // ──────────────────────────────────────────────
    // DecreaseStock
    // ──────────────────────────────────────────────

    [Fact]
    public void DecreaseStock_ShouldDecreaseStockCount()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "Test Product", 50M);
        product.IncreaseStock(10);
        int decreaseAmount = 4;

        // Act
        product.DecreaseStock(decreaseAmount);

        // Assert
        Assert.Equal(6, product.StockCount);
    }

    [Fact]
    public void DecreaseStock_ByExactStockAmount_ShouldResultInZeroStock()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "Test Product", 50M);
        product.IncreaseStock(5);

        // Act
        product.DecreaseStock(5);

        // Assert
        Assert.Equal(0, product.StockCount);
    }

    [Fact]
    public void DecreaseStock_ByZero_ShouldNotChangeStockCount()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "Test Product", 50M);
        product.IncreaseStock(10);
        int stockBefore = product.StockCount;

        // Act
        product.DecreaseStock(0);

        // Assert
        Assert.Equal(stockBefore, product.StockCount);
    }

    [Fact]
    public void DecreaseStock_BelowZero_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "Test Product", 50M);
        product.IncreaseStock(3);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => product.DecreaseStock(5));
    }

    [Fact]
    public void DecreaseStock_WithNegativeAmount_ShouldThrowArgumentException()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "Test Product", 50M);
        product.IncreaseStock(10);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => product.DecreaseStock(-1));
    }

    [Fact]
    public void DecreaseStock_CalledMultipleTimes_ShouldAccumulateDecrements()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "Test Product", 50M);
        product.IncreaseStock(20);

        // Act
        product.DecreaseStock(5);
        product.DecreaseStock(5);
        product.DecreaseStock(5);

        // Assert
        Assert.Equal(5, product.StockCount);
    }

    [Theory]
    [InlineData(10, 1, 9)]
    [InlineData(10, 5, 5)]
    [InlineData(10, 10, 0)]
    public void DecreaseStock_WithVariousValidAmounts_ShouldSetCorrectStockCount(
        int initialStock, int decreaseAmount, int expectedStock)
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "Test Product", 50M);
        product.IncreaseStock(initialStock);

        // Act
        product.DecreaseStock(decreaseAmount);

        // Assert
        Assert.Equal(expectedStock, product.StockCount);
    }

    [Theory]
    [InlineData(10, 11)]
    [InlineData(0, 1)]
    [InlineData(5, 100)]
    public void DecreaseStock_WhenAmountExceedsStock_ShouldThrowInvalidOperationException(
        int initialStock, int decreaseAmount)
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "Test Product", 50M);
        product.IncreaseStock(initialStock);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => product.DecreaseStock(decreaseAmount));
    }

    // ──────────────────────────────────────────────
    // Legacy / previously existing tests
    // ──────────────────────────────────────────────

    [Fact]
    public void ThrowArgumentException_WhenUnitPriceIsNegative()
    {
        // Arrange
        var productId = Guid.NewGuid();
        string title = "AyBiEm 13 inch i7 1 TB 64 Gb laptop";
        decimal negativeUnitPrice = -1499.99M;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Product(productId, title, negativeUnitPrice));
        Assert.Throws<ArgumentException>(() => new Product(productId, title, 0M));
    }
}
