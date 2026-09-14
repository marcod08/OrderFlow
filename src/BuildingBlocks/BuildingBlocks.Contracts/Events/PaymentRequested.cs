namespace BuildingBlocks.Contracts.Events;

public record PaymentRequested(Guid OrderId, decimal Amount);