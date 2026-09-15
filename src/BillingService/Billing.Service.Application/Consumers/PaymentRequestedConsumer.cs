using Billing.Service.Application.Payments.ProcessPayment;
using BuildingBlocks.Contracts.Events;
using MassTransit;
using MediatR;

namespace Billing.Service.Application.Consumers;

public class PaymentRequestedConsumer(IMediator mediator) : IConsumer<PaymentRequested>
{
    public async Task Consume(ConsumeContext<PaymentRequested> context)
    {
        var message = context.Message;

        var isSuccessful = await mediator.Send(new ProcessPaymentCommand(message.OrderId, message.Amount), context.CancellationToken);

        if (isSuccessful)
        {
            await context.Publish(new PaymentProcessed(message.OrderId), context.CancellationToken);
        }
        else
        {
            await context.Publish(new PaymentFailed(message.OrderId, "Payment processing failed"), context.CancellationToken);
        }
    }
}