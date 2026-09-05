using Catalog.Service.Domain;
using Catalog.Service.Infrastructure.Repositories;
using FluentAssertions;

namespace Catalog.Service.IntegrationTests;

public class ProductRepositoryTests(PostgresTestFixture fixture) : IClassFixture<PostgresTestFixture>
{
    [Fact]
    public async Task AddAsync_ThenGetByIdAsync_ReturnCorrectProduct()
    {
        // arrange
        var repository = new ProductRepository(fixture.DbContext);
        var product = new Product("Tastiera Meccanica", 89.99m, 50);

        // act
        await repository.AddAsync(product, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        var result = await repository.GetByIdAsync(product.Id, CancellationToken.None);

        // assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Tastiera Meccanica");
        result!.Price.Should().Be(89.99m);
        result!.StockQuantity.Should().Be(50);
    }

    [Fact]
    public async Task ReserveStock_ThenSave_PersistUpdatedStockQuantity()
    {
        // arrange
        var repository = new ProductRepository(fixture.DbContext);
        var product = new Product("Mouse Gaming", 49.99m, 100);

        await repository.AddAsync(product, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        // act
        product.ReserveStock(10);
        await repository.SaveChangesAsync(CancellationToken.None);

        var result = await repository.GetByIdAsync(product.Id, CancellationToken.None);

        // assert
        result.Should().NotBeNull();
        result!.StockQuantity.Should().Be(90);
    }
}