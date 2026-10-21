using LoopLab.Domain.Orders;

namespace LoopLab.Application.Orders;

public sealed record CreateOrderRequest(string CustomerId, CustomerTier Tier, string ShippingCity);

public sealed record AddOrderLineRequest(string Sku, string ProductName, decimal UnitPrice, int Quantity, decimal WeightKg);

public sealed record OrderSummary(
    Guid OrderId,
    OrderStatus Status,
    int ItemCount,
    decimal Subtotal,
    decimal Discount,
    decimal ShippingCost,
    decimal Total);

public sealed record ValidationResult(bool IsValid, IReadOnlyList<string> Errors)
{
    public static ValidationResult Success() => new(true, []);
    public static ValidationResult Failure(IEnumerable<string> errors) => new(false, errors.ToList());
}
