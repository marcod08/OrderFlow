using Microsoft.EntityFrameworkCore;
using Ordering.Service.Infrastructure.Persistence;
using Testcontainers.PostgreSql;

namespace Ordering.Service.IntegrationTests;

public class PostgresTestFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:16")
        .WithDatabase("orderingdb_test")
        .WithUsername("postgres")
        .WithPassword("testpassword")
        .Build();

    public OrderingDbContext DbContext { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var options = new DbContextOptionsBuilder<OrderingDbContext>()
            .UseNpgsql(_container.GetConnectionString())
            .Options;

        DbContext = new OrderingDbContext(options);
        await DbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
        await _container.DisposeAsync();
    }
}