using MediatR;

namespace Catalog.Service.Application.Products.UpdateProductStock;

public record ReserveProductStockCommand (Guid ProductId, int Quantity) : IRequest<bool>;