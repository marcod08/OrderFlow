using Ordering.Service.Domain;

namespace Ordering.Service.Application.DTOs;

public record OrderResponse(
    Guid Id,
    Guid ProductId,
    int Quantity,
    decimal? TotalPrice,
    OrderStatus Status,
    DateTime CreatedAt
);