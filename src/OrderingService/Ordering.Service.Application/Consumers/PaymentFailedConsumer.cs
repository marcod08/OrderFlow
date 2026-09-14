using BuildingBlocks.Contracts.Events;
using MassTransit;
using Ordering.Service.Application.Interfaces;

namespace Ordering.Service.Application.Consumers;

public class PaymentFailedConsumer(IOrderRepository repository) : IConsumer<PaymentFailed>
{
    public async Task Consume(ConsumeContext<PaymentFailed> context)
    {
        var message = context.Message;

        var order = await repository.GetByIdAsync(message.OrderId, context.CancellationToken) ?? throw new KeyNotFoundException($"Order with id {message.OrderId} not found");

        order.MarkCancelled();

        await repository.SaveChangesAsync(context.CancellationToken);
    }
}