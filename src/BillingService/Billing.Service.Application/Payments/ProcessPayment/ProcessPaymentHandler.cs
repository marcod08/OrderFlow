using Billing.Service.Application.Interfaces;
using Billing.Service.Domain;
using MediatR;

namespace Billing.Service.Application.Payments.ProcessPayment;

public class ProcessPaymentHandler(IPaymentRepository repository) : IRequestHandler<ProcessPaymentCommand, bool>
{
    public async Task<bool> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = new Payment(request.OrderId, request.Amount);

        var isSuccessful = Random.Shared.Next(100) < 90; // Simulate an 90% chance of success

        if (isSuccessful)
        {
            payment.MarkCompleted();
        }
        else
        {
            payment.MarkFailed();
        }

        await repository.AddAsync(payment, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return isSuccessful;
    }

}