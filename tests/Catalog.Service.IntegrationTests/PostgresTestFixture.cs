using Catalog.Service.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Catalog.Service.IntegrationTests;

public class PostgresTestFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:16")
        .WithDatabase("catalogdb_test")
        .WithUsername("postgres")
        .WithPassword("testpassword")
        .Build();
        
    public CatalogDbContext DbContext { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseNpgsql(_container.GetConnectionString())
            .Options;

        DbContext = new CatalogDbContext(options);
        await DbContext.Database.MigrateAsync();
    }
        public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
        await _container.DisposeAsync();
    }
}