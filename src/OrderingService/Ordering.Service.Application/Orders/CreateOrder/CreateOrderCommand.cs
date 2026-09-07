using MediatR;

namespace Ordering.Service.Application.Orders.CreateOrder;

public record CreateOrderCommand (Guid ProductId, int Quantity) : IRequest<Guid>;