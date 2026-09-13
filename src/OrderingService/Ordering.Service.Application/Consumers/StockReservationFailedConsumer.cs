using BuildingBlocks.Contracts.Events;
using MassTransit;
using Ordering.Service.Application.Interfaces;

namespace Ordering.Service.Application.Consumers;

public class StockReservationFailedConsumer(IOrderRepository repository) : IConsumer<StockReservationFailed>
{
    public async Task Consume(ConsumeContext<StockReservationFailed> context)
    {
        var message = context.Message;

        var order = await repository.GetByIdAsync(message.OrderId, context.CancellationToken) ?? throw new KeyNotFoundException($"Order with id {message.OrderId} not found");

        order.MarkCancelled();

        await repository.SaveChangesAsync(context.CancellationToken);
    }
}