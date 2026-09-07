using MediatR;
using Ordering.Service.Application.Interfaces;
using Ordering.Service.Domain;

namespace Ordering.Service.Application.Orders.CreateOrder;

public class CreateOrderHandler(IOrderRepository repository) : IRequestHandler<CreateOrderCommand, Guid>
{
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order(request.ProductId, request.Quantity);

        await repository.AddAsync(order, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}