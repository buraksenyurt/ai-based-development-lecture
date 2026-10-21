using System.Collections.Concurrent;
using LoopLab.Application.Abstractions;
using LoopLab.Domain.Orders;

namespace LoopLab.Infrastructure.Persistence;

/// <summary>
/// Demo amaçlı bellek içi depo. Uygulama yeniden başlatıldığında veriler kaybolur.
/// </summary>
public sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(_orders.GetValueOrDefault(id));

    public Task<IReadOnlyList<Order>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Order> result = _orders.Values
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.CreatedAtUtc)
            .ToList();
        return Task.FromResult(result);
    }

    public Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        if (!_orders.TryAdd(order.Id, order))
            throw new InvalidOperationException($"{order.Id} zaten kayıtlı.");
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        _orders[order.Id] = order;
        return Task.CompletedTask;
    }
}
