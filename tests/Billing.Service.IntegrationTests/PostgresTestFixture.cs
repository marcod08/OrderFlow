using Billing.Service.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Billing.Service.IntegrationTests;

public class PostgresTestFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:16")
        .WithDatabase("billingdb_test")
        .WithUsername("postgres")
        .WithPassword("testpassword")
        .Build();
        
    public BillingDbContext DbContext { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        
        var options = new DbContextOptionsBuilder<BillingDbContext>()
            .UseNpgsql(_container.GetConnectionString())
            .Options;

        DbContext = new BillingDbContext(options);
        await DbContext.Database.MigrateAsync();
    }
    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
        await _container.DisposeAsync();
    }
}