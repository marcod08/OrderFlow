using Catalog.Service.Application.Interfaces;
using MediatR;

namespace Catalog.Service.Application.Products.UpdateProductStock;

public class ReserveProductStockHandler (IProductRepository repository) : IRequestHandler<ReserveProductStockCommand, bool>
{
    public async Task<bool> Handle(ReserveProductStockCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.ProductId, cancellationToken) ?? throw new KeyNotFoundException($"Product with ID {request.ProductId} not found.");
        product.ReserveStock(request.Quantity);

        await repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}