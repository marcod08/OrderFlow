using MediatR;

namespace Ordering.Service.Application.Orders.CancelOrder;

public record CancelOrderCommand (Guid OrderId) :IRequest<bool>;