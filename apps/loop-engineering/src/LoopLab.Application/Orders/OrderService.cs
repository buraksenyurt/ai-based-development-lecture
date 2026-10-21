using LoopLab.Application.Abstractions;
using LoopLab.Domain.Common;
using LoopLab.Domain.Orders;

namespace LoopLab.Application.Orders;

public sealed class OrderService(
    IOrderRepository repository,
    OrderDiscountCalculator discountCalculator,
    ShippingCostCalculator shippingCalculator,
    OrderValidator validator)
{
    public async Task<Guid> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var order = new Order(request.CustomerId, request.Tier, request.ShippingCity, DateTime.UtcNow);
        await repository.AddAsync(order, cancellationToken);
        return order.Id;
    }

    public async Task AddLineAsync(Guid orderId, AddOrderLineRequest request, CancellationToken cancellationToken = default)
    {
        var order = await GetOrThrowAsync(orderId, cancellationToken);
        order.AddLine(new OrderLine(request.Sku, request.ProductName, request.UnitPrice, request.Quantity, request.WeightKg));
        await repository.UpdateAsync(order, cancellationToken);
    }

    public async Task ApplyCouponAsync(Guid orderId, string couponCode, CancellationToken cancellationToken = default)
    {
        var order = await GetOrThrowAsync(orderId, cancellationToken);
        order.ApplyCoupon(couponCode);
        await repository.UpdateAsync(order, cancellationToken);
    }

    public async Task<OrderSummary> GetSummaryAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await GetOrThrowAsync(orderId, cancellationToken);
        return BuildSummary(order);
    }

    public async Task<OrderSummary> PlaceAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await GetOrThrowAsync(orderId, cancellationToken);

        var validation = validator.Validate(order);
        if (!validation.IsValid)
            throw new DomainException(string.Join(" ", validation.Errors));

        order.Place(DateTime.UtcNow);
        await repository.UpdateAsync(order, cancellationToken);
        return BuildSummary(order);
    }

    public async Task CancelAsync(Guid orderId, string reason, CancellationToken cancellationToken = default)
    {
        var order = await GetOrThrowAsync(orderId, cancellationToken);
        order.Cancel(reason);
        await repository.UpdateAsync(order, cancellationToken);
    }

    private OrderSummary BuildSummary(Order order)
    {
        var discount = discountCalculator.Calculate(order);
        var discounted = order.Subtotal - discount;
        var shipping = shippingCalculator.Calculate(order, discounted);

        return new OrderSummary(
            order.Id,
            order.Status,
            order.ItemCount,
            order.Subtotal,
            discount,
            shipping,
            discounted + shipping);
    }

    private async Task<Order> GetOrThrowAsync(Guid orderId, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(orderId, cancellationToken) ?? throw new OrderNotFoundException(orderId);
}
