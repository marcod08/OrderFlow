namespace Billing.Service.Domain;

public enum PaymentStatus
{
    Pending,
    Completed,
    Failed
}
public class Payment
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Payment() { } //lo userò per EF Core;

    public Payment(Guid orderId, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
        }

        Id = Guid.NewGuid();
        OrderId = orderId;
        Amount = amount;
        Status = PaymentStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkCompleted()
    {
        if (Status != PaymentStatus.Pending)
        {
            throw new InvalidOperationException("Payment must be in Pending status to mark as completed.");
        }

        Status = PaymentStatus.Completed;
    }

    public void MarkFailed()
    {
        if (Status != PaymentStatus.Pending)
        {
            throw new InvalidOperationException("Payment must be in Pending status to mark as failed.");
        }

        Status = PaymentStatus.Failed;
    }
}