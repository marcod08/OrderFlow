using Catalog.Service.Application.Interfaces;
using Catalog.Service.Domain;
using Catalog.Service.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Service.Infrastructure.Repositories;

public class ProductRepository(CatalogDbContext context) : IProductRepository
{   
    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        await context.Products.AddAsync(product, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Products.ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteAsync(Product product, CancellationToken cancellationToken)
    {
        context.Products.Remove(product);
        return Task.CompletedTask;
    }
}