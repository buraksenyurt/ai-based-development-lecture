using LoopLab.Domain.Common;

namespace LoopLab.Domain.Orders;

public sealed class Order
{
    public const int MaxDistinctLines = 20;

    private readonly List<OrderLine> _lines = [];

    public Guid Id { get; }
    public string CustomerId { get; }
    public CustomerTier CustomerTier { get; }
    public string ShippingCity { get; }
    public OrderStatus Status { get; private set; } = OrderStatus.Draft;
    public string? CouponCode { get; private set; }
    public DateTime CreatedAtUtc { get; }
    public DateTime? PlacedAtUtc { get; private set; }
    public string? CancellationReason { get; private set; }

    public IReadOnlyList<OrderLine> Lines => _lines.AsReadOnly();
    public decimal Subtotal => _lines.Sum(l => l.LineTotal);
    public decimal TotalWeightKg => _lines.Sum(l => l.TotalWeightKg);
    public int ItemCount => _lines.Sum(l => l.Quantity);

    public Order(string customerId, CustomerTier tier, string shippingCity, DateTime createdAtUtc)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            throw new DomainException("Müşteri kimliği boş olamaz.");
        if (string.IsNullOrWhiteSpace(shippingCity))
            throw new DomainException("Teslimat şehri boş olamaz.");

        Id = Guid.NewGuid();
        CustomerId = customerId;
        CustomerTier = tier;
        ShippingCity = shippingCity.Trim();
        CreatedAtUtc = createdAtUtc;
    }

    public void AddLine(OrderLine line)
    {
        EnsureDraft();

        var existing = _lines.FirstOrDefault(l => l.Sku == line.Sku);
        if (existing is not null)
        {
            if (existing.UnitPrice != line.UnitPrice)
                throw new DomainException($"{line.Sku} için farklı birim fiyat ile satır eklenemez.");
            existing.IncreaseQuantity(line.Quantity);
            return;
        }

        if (_lines.Count >= MaxDistinctLines)
            throw new DomainException($"Bir siparişte en fazla {MaxDistinctLines} farklı ürün olabilir.");

        _lines.Add(line);
    }

    public void RemoveLine(string sku)
    {
        EnsureDraft();
        var removed = _lines.RemoveAll(l => l.Sku == sku.Trim().ToUpperInvariant());
        if (removed == 0)
            throw new DomainException($"{sku} siparişte bulunamadı.");
    }

    public void ApplyCoupon(string couponCode)
    {
        EnsureDraft();
        if (string.IsNullOrWhiteSpace(couponCode))
            throw new DomainException("Kupon kodu boş olamaz.");
        CouponCode = couponCode.Trim().ToUpperInvariant();
    }

    public void Place(DateTime placedAtUtc)
    {
        EnsureDraft();
        if (_lines.Count == 0)
            throw new DomainException("Boş sipariş onaylanamaz.");
        Status = OrderStatus.Placed;
        PlacedAtUtc = placedAtUtc;
    }

    public void Ship()
    {
        if (Status != OrderStatus.Placed)
            throw new DomainException("Sadece onaylanmış siparişler kargoya verilebilir.");
        Status = OrderStatus.Shipped;
    }

    public void Deliver()
    {
        if (Status != OrderStatus.Shipped)
            throw new DomainException("Sadece kargodaki siparişler teslim edilebilir.");
        Status = OrderStatus.Delivered;
    }

    public void Cancel(string reason)
    {
        if (Status is OrderStatus.Shipped or OrderStatus.Delivered)
            throw new DomainException("Kargoya verilmiş sipariş iptal edilemez.");
        if (Status == OrderStatus.Cancelled)
            throw new DomainException("Sipariş zaten iptal edilmiş.");
        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException("İptal nedeni belirtilmelidir.");

        Status = OrderStatus.Cancelled;
        CancellationReason = reason;
    }

    private void EnsureDraft()
    {
        if (Status != OrderStatus.Draft)
            throw new DomainException("Sadece taslak durumundaki siparişler değiştirilebilir.");
    }
}
