using MediatR;
using Ordering.Service.Application.Interfaces;

namespace Ordering.Service.Application.Orders.CancelOrder;

public class CancelOrderHandler(IOrderRepository repository) : IRequestHandler<CancelOrderCommand, bool>
{
    public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await repository.GetByIdAsync(request.OrderId, cancellationToken) ?? throw new KeyNotFoundException($"Order with id {request.OrderId} not found.");
        order.MarkCancelled();

        await repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}