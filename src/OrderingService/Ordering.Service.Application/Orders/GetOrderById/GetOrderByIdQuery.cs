using MediatR;
using Ordering.Service.Application.DTOs;

namespace Ordering.Service.Application.Orders.GetOrderById;

public record GetOrderByIdQuery(Guid Id) : IRequest<OrderResponse?>;