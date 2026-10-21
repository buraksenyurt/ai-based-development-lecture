using LoopLab.Domain.Orders;

namespace LoopLab.Application.Orders;

/// <summary>
/// Ağırlık, şehir ve sipariş tutarına göre kargo ücretini hesaplar.
/// </summary>
public sealed class ShippingCostCalculator
{
    public const decimal FreeShippingThreshold = 750m;
    public const decimal BaseCost = 49.90m;
    public const decimal PerExtraKg = 7.50m;
    public const decimal IncludedKg = 2m;

    private static readonly HashSet<string> RemoteCities =
        new(StringComparer.OrdinalIgnoreCase) { "Hakkari", "Şırnak", "Ardahan", "Iğdır" };

    public decimal Calculate(Order order, decimal discountedSubtotal)
    {
        ArgumentNullException.ThrowIfNull(order);

        if (order.Lines.Count == 0)
            return 0m;

        if (discountedSubtotal >= FreeShippingThreshold && order.CustomerTier != CustomerTier.Standard)
            return 0m;

        var cost = BaseCost;

        var extraKg = order.TotalWeightKg - IncludedKg;
        if (extraKg > 0)
            cost += Math.Ceiling(extraKg) * PerExtraKg;

        if (RemoteCities.Contains(order.ShippingCity))
            cost *= 1.25m;

        if (discountedSubtotal >= FreeShippingThreshold)
            cost *= 0.5m;

        return Math.Round(cost, 2, MidpointRounding.AwayFromZero);
    }
}
