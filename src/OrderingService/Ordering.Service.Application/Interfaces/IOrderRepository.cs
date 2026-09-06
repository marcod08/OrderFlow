using Ordering.Service.Domain;

namespace Ordering.Service.Application.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Order order, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken);
    Task DeleteAsync(Order order, CancellationToken cancellationToken);
}