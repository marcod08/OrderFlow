using MediatR;

namespace Ordering.Service.Application.Orders;

public record CreateOrderCommand (Guid ProductId, int Quantity) : IRequest<Guid>;