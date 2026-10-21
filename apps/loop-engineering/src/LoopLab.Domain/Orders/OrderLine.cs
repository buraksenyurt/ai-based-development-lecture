using LoopLab.Domain.Common;

namespace LoopLab.Domain.Orders;

public sealed class OrderLine
{
    public string Sku { get; }
    public string ProductName { get; }
    public decimal UnitPrice { get; }
    public int Quantity { get; private set; }
    public decimal WeightKg { get; }

    public decimal LineTotal => UnitPrice * Quantity;
    public decimal TotalWeightKg => WeightKg * Quantity;

    public OrderLine(string sku, string productName, decimal unitPrice, int quantity, decimal weightKg)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new DomainException("SKU boş olamaz.");
        if (unitPrice <= 0)
            throw new DomainException("Birim fiyat sıfırdan büyük olmalıdır.");
        if (quantity <= 0)
            throw new DomainException("Adet sıfırdan büyük olmalıdır.");
        if (weightKg < 0)
            throw new DomainException("Ağırlık negatif olamaz.");

        Sku = sku.Trim().ToUpperInvariant();
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
        WeightKg = weightKg;
    }

    internal void IncreaseQuantity(int amount)
    {
        if (amount <= 0)
            throw new DomainException("Artış miktarı sıfırdan büyük olmalıdır.");
        Quantity += amount;
    }
}
