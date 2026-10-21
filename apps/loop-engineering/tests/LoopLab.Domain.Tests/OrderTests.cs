using LoopLab.Domain.Common;
using LoopLab.Domain.Orders;

namespace LoopLab.Domain.Tests;

public class OrderTests
{
    private static Order NewOrder() =>
        new("C-001", CustomerTier.Standard, "İstanbul", new DateTime(2026, 10, 1, 9, 0, 0, DateTimeKind.Utc));

    [Fact]
    public void New_order_starts_as_draft_with_no_lines()
    {
        var order = NewOrder();

        Assert.Equal(OrderStatus.Draft, order.Status);
        Assert.Empty(order.Lines);
        Assert.Equal(0m, order.Subtotal);
    }

    [Fact]
    public void Adding_a_line_updates_subtotal()
    {
        var order = NewOrder();

        order.AddLine(new OrderLine("kb-01", "Klavye", 250m, 2, 0.8m));

        Assert.Single(order.Lines);
        Assert.Equal(500m, order.Subtotal);
    }

    [Fact]
    public void Placing_an_empty_order_is_rejected()
    {
        var order = NewOrder();

        Assert.Throws<DomainException>(() => order.Place(DateTime.UtcNow));
    }
}
