using BuildingBlocks.Contracts.Events;
using MassTransit;
using Ordering.Service.Application.Interfaces;

namespace Ordering.Service.Application.Consumers;

public class StockReservedConsumer(IOrderRepository repository) : IConsumer<StockReserved>
{
    public async Task Consume(ConsumeContext<StockReserved> context)
    {
        var message = context.Message;

        var order = await repository.GetByIdAsync(message.OrderId, context.CancellationToken) ?? throw new KeyNotFoundException($"Order with id {message.OrderId} not found");

        order.MarkStockReserved();

        await repository.SaveChangesAsync(context.CancellationToken);
    }
}