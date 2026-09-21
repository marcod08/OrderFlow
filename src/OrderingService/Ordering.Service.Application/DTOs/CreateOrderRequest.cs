namespace Ordering.Service.Application.DTOs;

public record CreateOrderRequest(Guid ProductId, int Quantity);