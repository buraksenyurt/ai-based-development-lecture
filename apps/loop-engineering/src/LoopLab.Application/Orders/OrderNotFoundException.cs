namespace LoopLab.Application.Orders;

public sealed class OrderNotFoundException(Guid orderId)
    : Exception($"{orderId} kimlikli sipariş bulunamadı.")
{
    public Guid OrderId { get; } = orderId;
}
