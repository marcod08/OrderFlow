using Catalog.Service.Domain;

namespace Catalog.Service.Application.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Product product, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken);

}