using MediatR;
using Ordering.Service.Application.DTOs;
using Ordering.Service.Application.Interfaces;

namespace Ordering.Service.Application.Orders.GetOrderById;

public class GetOrderByIdHandler (IOrderRepository repository) : IRequestHandler<GetOrderByIdQuery, OrderResponse?>
{
    public async Task<OrderResponse?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (order is null) return null;
        
        return new OrderResponse(order.Id, order.ProductId, order.Quantity, order.TotalPrice, order.Status, order.CreatedAt);
    }
}