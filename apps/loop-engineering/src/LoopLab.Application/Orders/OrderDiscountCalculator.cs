using LoopLab.Domain.Orders;

namespace LoopLab.Application.Orders;

/// <summary>
/// Sipariş için uygulanacak toplam indirimi hesaplar.
/// Kurallar: müşteri seviyesi, kupon, hafta sonu kampanyası ve Kasım ayı kampanyası.
/// Toplam indirim ara toplamın %40'ını geçemez.
/// </summary>
public sealed class OrderDiscountCalculator
{
    public const decimal MaxDiscountRate = 0.40m;

    private static readonly Dictionary<string, decimal> Coupons = new()
    {
        ["HOSGELDIN10"] = 0.10m,
        ["OGRENCI15"] = 0.15m,
        ["SADAKAT20"] = 0.20m
    };

    public decimal Calculate(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        var subtotal = order.Subtotal;
        if (subtotal <= 0)
            return 0m;

        var rate = GetTierRate(order.CustomerTier);
        rate += GetCouponRate(order.CouponCode, subtotal);

        var now = DateTime.Now;
        if (now.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday && subtotal >= 500m)
            rate += 0.05m;

        if (now.Month == 11 && now.Day >= 20)
            rate += 0.10m;

        if (rate > MaxDiscountRate)
            rate = MaxDiscountRate;

        return Math.Round(subtotal * rate, 2, MidpointRounding.AwayFromZero);
    }

    private static decimal GetTierRate(CustomerTier tier) => tier switch
    {
        CustomerTier.Silver => 0.03m,
        CustomerTier.Gold => 0.05m,
        CustomerTier.Platinum => 0.08m,
        _ => 0m
    };

    private static decimal GetCouponRate(string? couponCode, decimal subtotal)
    {
        if (couponCode is null)
            return 0m;
        if (!Coupons.TryGetValue(couponCode, out var rate))
            return 0m;
        if (couponCode == "SADAKAT20" && subtotal < 1000m)
            return 0m;
        return rate;
    }
}
