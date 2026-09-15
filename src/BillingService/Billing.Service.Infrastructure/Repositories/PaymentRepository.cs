using Billing.Service.Application.Interfaces;
using Billing.Service.Domain;
using Billing.Service.Infrastructure.Persistence;

namespace Billing.Service.Infrastructure.Repositories;

public class PaymentRepository(BillingDbContext context) : IPaymentRepository
{
    public async Task AddAsync(Payment payment, CancellationToken cancellationToken)
    {
        await context.Payments.AddAsync(payment, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}