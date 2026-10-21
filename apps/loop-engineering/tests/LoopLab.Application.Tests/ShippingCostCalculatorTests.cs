using LoopLab.Application.Orders;
using LoopLab.Domain.Orders;

namespace LoopLab.Application.Tests;

public class ShippingCostCalculatorTests
{
    private readonly ShippingCostCalculator _calculator = new();

    [Fact]
    public void Empty_order_has_no_shipping_cost()
    {
        var order = new Order("C-001", CustomerTier.Standard, "Ankara", DateTime.UtcNow);

        Assert.Equal(0m, _calculator.Calculate(order, 0m));
    }

    [Fact]
    public void Light_order_pays_base_cost()
    {
        var order = new Order("C-001", CustomerTier.Standard, "Ankara", DateTime.UtcNow);
        order.AddLine(new OrderLine("MS-01", "Mouse", 150m, 1, 0.3m));

        Assert.Equal(ShippingCostCalculator.BaseCost, _calculator.Calculate(order, 150m));
    }
}
