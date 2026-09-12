namespace BuildingBlocks.Contracts.Events;

public record OrderCreated (Guid OrderId, Guid ProductId, int Quantity);
