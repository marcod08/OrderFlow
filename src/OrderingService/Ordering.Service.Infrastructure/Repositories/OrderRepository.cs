using Microsoft.EntityFrameworkCore;
using Ordering.Service.Application.Interfaces;
using Ordering.Service.Domain;
using Ordering.Service.Infrastructure.Persistence;

namespace Ordering.Service.Infrastructure.Repositories;

public class OrderRepository(OrderingDbContext context) : IOrderRepository
{
    public async Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        await context.Orders.AddAsync(order, cancellationToken);
    }

    public Task DeleteAsync(Order order, CancellationToken cancellationToken)
    {
        context.Orders.Remove(order);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Orders.ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Orders.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}