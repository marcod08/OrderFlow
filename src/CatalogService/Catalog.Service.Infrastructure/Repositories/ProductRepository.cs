using Catalog.Service.Application.Interfaces;
using Catalog.Service.Domain;
using Catalog.Service.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Service.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{   
    private readonly CatalogDbContext _context;

    public ProductRepository(CatalogDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        await _context.Products.AddAsync(product, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Products.ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}