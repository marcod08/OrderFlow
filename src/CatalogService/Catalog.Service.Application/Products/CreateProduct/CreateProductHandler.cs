using Catalog.Service.Application.Interfaces;
using Catalog.Service.Domain;
using MediatR;

namespace Catalog.Service.Application.Products.CreateProduct;

public class CreateProductHandler (IProductRepository repository) : IRequestHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product(request.Name, request.Price, request.InitialStock);

        await repository.AddAsync(product, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}