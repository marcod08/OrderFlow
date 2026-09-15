using Billing.Service.Domain;
using Microsoft.EntityFrameworkCore;

namespace Billing.Service.Infrastructure.Persistence;

public class BillingDbContext(DbContextOptions<BillingDbContext> options) : DbContext(options)
{
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BillingDbContext).Assembly);
    }
}