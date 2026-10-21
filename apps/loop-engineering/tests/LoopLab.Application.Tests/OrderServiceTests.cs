using LoopLab.Application.Abstractions;
using LoopLab.Application.Orders;
using LoopLab.Domain.Common;
using LoopLab.Domain.Orders;

namespace LoopLab.Application.Tests;

public class OrderServiceTests
{
    private readonly FakeOrderRepository _repository = new();
    private readonly OrderService _service;

    public OrderServiceTests()
    {
        _service = new OrderService(
            _repository,
            new OrderDiscountCalculator(),
            new ShippingCostCalculator(),
            new OrderValidator());
    }

    private Task<Guid> CreateOrderAsync(CustomerTier tier = CustomerTier.Standard) =>
        _service.CreateAsync(new CreateOrderRequest("C-001", tier, "Ankara"));

    // 2 x 60 TL, 2 x 0,5 kg: ara toplam 120 TL, ağırlık 1 kg (kargo eşiklerinin altında).
    private static AddOrderLineRequest KeyboardLine() => new("kb-01", "Klavye", 60m, 2, 0.5m);

    [Fact]
    public async Task CreateAsync_stores_draft_order_and_returns_its_id()
    {
        var id = await _service.CreateAsync(new CreateOrderRequest("C-042", CustomerTier.Gold, " İzmir "));

        var stored = Assert.Single(_repository.Orders);
        Assert.Equal(id, stored.Id);
        Assert.Equal("C-042", stored.CustomerId);
        Assert.Equal(CustomerTier.Gold, stored.CustomerTier);
        Assert.Equal("İzmir", stored.ShippingCity);
        Assert.Equal(OrderStatus.Draft, stored.Status);
    }

    [Fact]
    public async Task AddLineAsync_adds_line_to_order_and_persists_it()
    {
        var id = await CreateOrderAsync();

        await _service.AddLineAsync(id, KeyboardLine());

        var order = _repository.Get(id);
        var line = Assert.Single(order.Lines);
        Assert.Equal("KB-01", line.Sku);
        Assert.Equal(2, line.Quantity);
        Assert.Equal(120m, order.Subtotal);
        Assert.Equal(1, _repository.UpdateCount);
    }

    [Fact]
    public async Task ApplyCouponAsync_sets_normalized_coupon_and_persists_order()
    {
        var id = await CreateOrderAsync();

        await _service.ApplyCouponAsync(id, " hosgeldin10 ");

        Assert.Equal("HOSGELDIN10", _repository.Get(id).CouponCode);
        Assert.Equal(1, _repository.UpdateCount);
    }

    [Fact]
    public async Task GetSummaryAsync_returns_order_totals_with_shipping_and_consistent_total()
    {
        var id = await CreateOrderAsync();
        await _service.AddLineAsync(id, KeyboardLine());

        var summary = await _service.GetSummaryAsync(id);

        Assert.Equal(id, summary.OrderId);
        Assert.Equal(OrderStatus.Draft, summary.Status);
        Assert.Equal(2, summary.ItemCount);
        Assert.Equal(120m, summary.Subtotal);
        Assert.Equal(ShippingCostCalculator.BaseCost, summary.ShippingCost);
        // İndirim takvime bağlı kampanyalar içerebildiği için kesin değer yerine sınır ve bağıntı doğrulanır.
        Assert.InRange(summary.Discount, 0m, summary.Subtotal * OrderDiscountCalculator.MaxDiscountRate);
        Assert.Equal(summary.Subtotal - summary.Discount + summary.ShippingCost, summary.Total);
    }

    [Fact]
    public async Task PlaceAsync_places_valid_order_and_returns_placed_summary()
    {
        var id = await CreateOrderAsync();
        await _service.AddLineAsync(id, KeyboardLine());

        var summary = await _service.PlaceAsync(id);

        var order = _repository.Get(id);
        Assert.Equal(OrderStatus.Placed, order.Status);
        Assert.NotNull(order.PlacedAtUtc);
        Assert.Equal(OrderStatus.Placed, summary.Status);
        Assert.Equal(120m, summary.Subtotal);
        Assert.Equal(2, _repository.UpdateCount);
    }

    [Fact]
    public async Task PlaceAsync_throws_domain_exception_with_validation_errors_for_invalid_order()
    {
        var id = await CreateOrderAsync();

        var ex = await Assert.ThrowsAsync<DomainException>(() => _service.PlaceAsync(id));

        Assert.Contains("Sipariş en az bir ürün içermelidir.", ex.Message);
        Assert.Contains($"Minimum sipariş tutarı {OrderValidator.MinimumOrderAmount} TL'dir.", ex.Message);
        Assert.Equal(OrderStatus.Draft, _repository.Get(id).Status);
        Assert.Equal(0, _repository.UpdateCount);
    }

    [Fact]
    public async Task CancelAsync_cancels_order_with_reason_and_persists_it()
    {
        var id = await CreateOrderAsync();

        await _service.CancelAsync(id, "Müşteri vazgeçti");

        var order = _repository.Get(id);
        Assert.Equal(OrderStatus.Cancelled, order.Status);
        Assert.Equal("Müşteri vazgeçti", order.CancellationReason);
        Assert.Equal(1, _repository.UpdateCount);
    }

    [Fact]
    public async Task Operations_on_unknown_order_throw_order_not_found_exception()
    {
        var unknownId = Guid.NewGuid();

        var ex = await Assert.ThrowsAsync<OrderNotFoundException>(() => _service.GetSummaryAsync(unknownId));

        Assert.Equal(unknownId, ex.OrderId);
        Assert.Equal($"{unknownId} kimlikli sipariş bulunamadı.", ex.Message);
        await Assert.ThrowsAsync<OrderNotFoundException>(() => _service.AddLineAsync(unknownId, KeyboardLine()));
        await Assert.ThrowsAsync<OrderNotFoundException>(() => _service.ApplyCouponAsync(unknownId, "OGRENCI15"));
        await Assert.ThrowsAsync<OrderNotFoundException>(() => _service.PlaceAsync(unknownId));
        await Assert.ThrowsAsync<OrderNotFoundException>(() => _service.CancelAsync(unknownId, "Neden"));
        Assert.Equal(0, _repository.UpdateCount);
    }

    private sealed class FakeOrderRepository : IOrderRepository
    {
        private readonly Dictionary<Guid, Order> _orders = [];

        public IReadOnlyCollection<Order> Orders => _orders.Values;
        public int UpdateCount { get; private set; }

        public Order Get(Guid id) => _orders[id];

        public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => Task.FromResult(_orders.GetValueOrDefault(id));

        public Task<IReadOnlyList<Order>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default)
            => Task.FromResult<IReadOnlyList<Order>>(_orders.Values.Where(o => o.CustomerId == customerId).ToList());

        public Task AddAsync(Order order, CancellationToken cancellationToken = default)
        {
            _orders.Add(order.Id, order);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
        {
            _orders[order.Id] = order;
            UpdateCount++;
            return Task.CompletedTask;
        }
    }
}
