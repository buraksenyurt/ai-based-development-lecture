using LoopLab.Application.Orders;
using LoopLab.Domain.Orders;

namespace LoopLab.Application.Tests;

public class OrderValidatorTests
{
    private readonly OrderValidator _validator = new();

    private static Order NewOrder(CustomerTier tier = CustomerTier.Standard) =>
        new("C-001", tier, "Ankara", new DateTime(2026, 10, 1, 9, 0, 0, DateTimeKind.Utc));

    [Fact]
    public void Valid_order_passes_without_errors()
    {
        var order = NewOrder();
        order.AddLine(new OrderLine("KB-01", "Klavye", 100m, 1, 1m));

        var result = _validator.Validate(order);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Null_order_throws_argument_null_exception()
    {
        Assert.Throws<ArgumentNullException>(() => _validator.Validate(null!));
    }

    [Fact]
    public void Empty_order_is_invalid_with_no_items_and_minimum_amount_errors()
    {
        var result = _validator.Validate(NewOrder());

        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);
        Assert.Contains("Sipariş en az bir ürün içermelidir.", result.Errors);
        Assert.Contains($"Minimum sipariş tutarı {OrderValidator.MinimumOrderAmount} TL'dir.", result.Errors);
    }

    [Fact]
    public void Order_below_minimum_amount_is_invalid()
    {
        var order = NewOrder();
        order.AddLine(new OrderLine("MS-01", "Mouse", 49.99m, 1, 0.3m));

        var result = _validator.Validate(order);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal($"Minimum sipariş tutarı {OrderValidator.MinimumOrderAmount} TL'dir.", error);
    }

    [Fact]
    public void Order_exactly_at_minimum_amount_is_valid()
    {
        var order = NewOrder();
        order.AddLine(new OrderLine("MS-01", "Mouse", OrderValidator.MinimumOrderAmount, 1, 0.3m));

        Assert.True(_validator.Validate(order).IsValid);
    }

    [Fact]
    public void Line_exceeding_max_quantity_is_reported_with_its_sku()
    {
        var order = NewOrder();
        order.AddLine(new OrderLine("KB-01", "Klavye", 10m, OrderValidator.MaxQuantityPerLine + 1, 0.1m));
        order.AddLine(new OrderLine("MS-01", "Mouse", 10m, OrderValidator.MaxQuantityPerLine, 0.1m));

        var result = _validator.Validate(order);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal($"KB-01 için en fazla {OrderValidator.MaxQuantityPerLine} adet sipariş verilebilir.", error);
    }

    [Fact]
    public void Order_heavier_than_max_weight_is_invalid()
    {
        var order = NewOrder();
        order.AddLine(new OrderLine("TV-01", "Televizyon", 100m, 1, OrderValidator.MaxWeightKg + 0.5m));

        var result = _validator.Validate(order);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal($"Toplam ağırlık {OrderValidator.MaxWeightKg} kg sınırını aşıyor.", error);
    }

    [Fact]
    public void Order_exactly_at_max_weight_is_valid()
    {
        var order = NewOrder();
        order.AddLine(new OrderLine("TV-01", "Televizyon", 100m, 1, OrderValidator.MaxWeightKg));

        Assert.True(_validator.Validate(order).IsValid);
    }

    [Fact]
    public void Standard_customer_cannot_use_loyalty_coupon()
    {
        var order = NewOrder(CustomerTier.Standard);
        order.AddLine(new OrderLine("KB-01", "Klavye", 100m, 1, 1m));
        order.ApplyCoupon("sadakat20");

        var result = _validator.Validate(order);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal("SADAKAT20 kuponu sadece sadakat programı üyeleri içindir.", error);
    }

    [Theory]
    [InlineData(CustomerTier.Silver)]
    [InlineData(CustomerTier.Gold)]
    [InlineData(CustomerTier.Platinum)]
    public void Loyalty_members_can_use_loyalty_coupon(CustomerTier tier)
    {
        var order = NewOrder(tier);
        order.AddLine(new OrderLine("KB-01", "Klavye", 100m, 1, 1m));
        order.ApplyCoupon("SADAKAT20");

        Assert.True(_validator.Validate(order).IsValid);
    }

    [Fact]
    public void Multiple_rule_violations_are_all_reported()
    {
        var order = NewOrder(CustomerTier.Standard);
        order.AddLine(new OrderLine("KB-01", "Klavye", 1m, 11, 3m));
        order.ApplyCoupon("SADAKAT20");

        var result = _validator.Validate(order);

        Assert.False(result.IsValid);
        Assert.Equal(4, result.Errors.Count);
    }
}
