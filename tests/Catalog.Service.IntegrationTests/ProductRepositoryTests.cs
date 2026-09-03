using Catalog.Service.Domain;
using Catalog.Service.Infrastructure.Repositories;
using FluentAssertions;

namespace Catalog.Service.IntegrationTests;

public class ProductRepositoryTests : IClassFixture<PostgresTestFixture>
{
    private readonly PostgresTestFixture _fixture;

    public ProductRepositoryTests(PostgresTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task AddAsync_ThenGetByIdAsync_ReturnCorrectProduct()
    {
        // arrange
        var repository = new ProductRepository(_fixture.DbContext);
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
}