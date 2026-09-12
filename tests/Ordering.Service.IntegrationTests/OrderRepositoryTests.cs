using Ordering.Service.Domain;
using Ordering.Service.Infrastructure.Repositories;
using FluentAssertions;

namespace Ordering.Service.IntegrationTests;

public class OrderRepositoryTests(PostgresTestFixture fixture) : IClassFixture<PostgresTestFixture>
{
    [Fact]
    public async Task AddAsync_ThenGetByIdAsync_ReturnsCorrectOrder()
    {
        // Arrange
        var repository = new OrderRepository(fixture.DbContext);
        var order = new Order(Guid.NewGuid(), 5);

        // Act
        await repository.AddAsync(order, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        var result = await repository.GetByIdAsync(order.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(order.Id);
        result.Quantity.Should().Be(order.Quantity);
    }

    [Fact]
    public async Task MarkStockReserved_ThenSave_PersistUpdateStatus()
    {
        // Arrange
        var repository = new OrderRepository(fixture.DbContext);
        var order = new Order(Guid.NewGuid(), 5);

        await repository.AddAsync(order, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        // Act
        order.MarkStockReserved();
        await repository.SaveChangesAsync(CancellationToken.None);

        var result = await repository.GetByIdAsync(order.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(order.Id);
        result.Quantity.Should().Be(order.Quantity);
        result!.Status.Should().Be(OrderStatus.StockReserved);
    }
}
