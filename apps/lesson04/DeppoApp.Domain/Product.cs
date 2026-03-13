namespace DeppoApp.Domain;

public class Product
{
    public Guid ProductId { get; private set; }
    public string Title { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int StockCount { get; private set; }
    public Product(Guid id, string title, decimal unitPrice)
    {
        ProductId = id;
        Title = title;

        if (unitPrice <= 0) throw new ArgumentException("Unit price must be greater than zero.");

        UnitPrice = unitPrice;
    }
    public void IncreaseStock(int count)
    {
        if (count < 0) throw new ArgumentException("Increase amount cannot be negative.");

        StockCount += count;
    }
    public void DecreaseStock(int count)
    {
        if (count < 0) throw new ArgumentException("Decrease amount cannot be negative.");
        if (StockCount - count < 0) throw new InvalidOperationException("Cannot decrease stock below zero.");

        StockCount -= count;
    }
}
