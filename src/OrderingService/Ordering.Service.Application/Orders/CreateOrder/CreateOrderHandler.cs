using BuildingBlocks.Contracts.Events;
using MassTransit;
using MediatR;
using Ordering.Service.Application.Interfaces;
using Ordering.Service.Domain;

namespace Ordering.Service.Application.Orders.CreateOrder;

public class CreateOrderHandler(IOrderRepository repository, IPublishEndpoint publishEndpoint) : IRequestHandler<CreateOrderCommand, Guid>
{
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order(request.ProductId, request.Quantity);

        await repository.AddAsync(order, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new OrderCreated(order.Id, order.ProductId, order.Quantity), cancellationToken);

        return order.Id;
    }
}