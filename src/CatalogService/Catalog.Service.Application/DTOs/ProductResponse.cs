namespace Catalog.Service.Application.DTOs;

public record ProductResponse(Guid Id, string Name, decimal Price, int StockQuantity);
