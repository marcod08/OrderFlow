using BuildingBlocks.Contracts.Events;
using Catalog.Service.Application.Products.GetProductById;
using Catalog.Service.Application.Products.UpdateProductStock;
using MassTransit;
using MediatR;

namespace Catalog.Service.Application.Consumers;

public class OrderCreatedConsumer(IMediator mediator) : IConsumer<OrderCreated>
{
    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        var message = context.Message;

        try
        {
            await mediator.Send(new ReserveProductStockCommand(message.ProductId, message.Quantity), context.CancellationToken);

            var product = await mediator.Send(new GetProductByIdQuery(message.ProductId), context.CancellationToken);

            await context.Publish(new StockReserved(message.OrderId, product!.Price), context.CancellationToken);
        }
        catch (Exception ex)
        {
            await context.Publish(new StockReservationFailed(message.OrderId, ex.Message), context.CancellationToken);
        }
    }
}