namespace BuildingBlocks.Contracts.Events;

public record StockReservationFailed(Guid OrderId, string Reason);