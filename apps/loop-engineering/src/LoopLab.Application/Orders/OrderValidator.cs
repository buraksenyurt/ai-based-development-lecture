using LoopLab.Domain.Orders;

namespace LoopLab.Application.Orders;

/// <summary>
/// Sipariş onaylanmadan önce iş kurallarını kontrol eder.
/// </summary>
public sealed class OrderValidator
{
    public const decimal MinimumOrderAmount = 50m;
    public const int MaxQuantityPerLine = 10;
    public const decimal MaxWeightKg = 30m;

    public ValidationResult Validate(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        var errors = new List<string>();

        if (order.Lines.Count == 0)
            errors.Add("Sipariş en az bir ürün içermelidir.");

        if (order.Subtotal < MinimumOrderAmount)
            errors.Add($"Minimum sipariş tutarı {MinimumOrderAmount} TL'dir.");

        foreach (var line in order.Lines.Where(l => l.Quantity > MaxQuantityPerLine))
            errors.Add($"{line.Sku} için en fazla {MaxQuantityPerLine} adet sipariş verilebilir.");

        if (order.TotalWeightKg > MaxWeightKg)
            errors.Add($"Toplam ağırlık {MaxWeightKg} kg sınırını aşıyor.");

        if (order.CustomerTier == CustomerTier.Standard && order.CouponCode == "SADAKAT20")
            errors.Add("SADAKAT20 kuponu sadece sadakat programı üyeleri içindir.");

        return errors.Count == 0 ? ValidationResult.Success() : ValidationResult.Failure(errors);
    }
}
