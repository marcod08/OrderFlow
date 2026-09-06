namespace Ordering.Service.Domain;

public enum OrderStatus
{
    Pending,
    StockReserved,
    PaymentProcessed,
    Confirmed,
    Cancelled
}

public class Order
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal TotalPrice { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Order() { } //lo userò per EF Core

    public Order(Guid productId, int quantity, decimal totalPrice)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        if (totalPrice < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalPrice), "Total price cannot be negative.");
        }

        Id = Guid.NewGuid();
        ProductId = productId;
        Quantity = quantity;
        TotalPrice = totalPrice;
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkStockReserved()
    {
        if (Status != OrderStatus.Pending)
        {
            throw new InvalidOperationException("Order must be in Pending status to mark stock as reserved.");
        }

        Status = OrderStatus.StockReserved;
    }

    public void MarkPaymentProcessed()
    {
        if (Status != OrderStatus.StockReserved)
        {
            throw new InvalidOperationException("Order must be in StockReserved status to mark payment as processed.");
        }

        Status = OrderStatus.PaymentProcessed;
    }

    public void MarkConfirmed()
    {
        if (Status != OrderStatus.PaymentProcessed)
        {
            throw new InvalidOperationException("Order must be in PaymentProcessed status to mark as confirmed.");
        }

        Status = OrderStatus.Confirmed;
    }

    public void MarkCancelled()
    {
        if (Status == OrderStatus.Confirmed || Status == OrderStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot cancel a confirmed order.");
        }

        Status = OrderStatus.Cancelled;
    }
}