using Catalog.Service.Application.Interfaces;
using MediatR;

namespace Catalog.Service.Application.Products.DeleteProduct;

public class DeleteProductHandler(IProductRepository repository) : IRequestHandler<DeleteProductCommand, bool>
{
    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.ProductId, cancellationToken) ?? throw new KeyNotFoundException($"Product with ID {request.ProductId} not found.");
        await repository.DeleteAsync(product, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}