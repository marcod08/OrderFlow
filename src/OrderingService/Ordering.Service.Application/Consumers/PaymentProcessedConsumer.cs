using BuildingBlocks.Contracts.Events;
using MassTransit;
using Ordering.Service.Application.Interfaces;

namespace Ordering.Service.Application.Consumers;

public class PaymentProcessedConsumer(IOrderRepository repository) : IConsumer<PaymentProcessed>
{
    public async Task Consume(ConsumeContext<PaymentProcessed> context)
    {
        var message = context.Message;

        var order = await repository.GetByIdAsync(message.OrderId, context.CancellationToken) ?? throw new KeyNotFoundException($"Order with id {message.OrderId} not found");
        
        order.MarkPaymentProcessed();
        order.MarkConfirmed();

        await repository.SaveChangesAsync(context.CancellationToken);
    }
}