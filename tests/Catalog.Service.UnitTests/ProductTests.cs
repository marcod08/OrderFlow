using Catalog.Service.Domain;
using FluentAssertions;

namespace Catalog.Service.UnitTests;

public class ProductTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesProductSuccessfully()
    {
        // Arrange
        var name = "Tastiera meccanica";
        var price = 89.99m;
        var initialStock = 50;

        //Act
        var product = new Product(name, price, initialStock);

        // Assert
        product.Name.Should().Be(name);
        product.Price.Should().Be(price);
        product.StockQuantity.Should().Be(initialStock);
        product.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Constructor_WithInvalidName_ThrowsArgumentException()
    {
        // Arrange
        var name = "";
        var price = 89.99m;
        var initialStock = 50;

        // Act
        Action act = () => new Product(name, price, initialStock);
        
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_WithInvalidPrice_ThrowsArgumentException()
    {
        // Arrange
        var name = "Tastiera meccanica";
        var price = -10.99m;
        var initialStock = 50;

        // Act
        Action act = () => new Product(name, price, initialStock);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void ReserveStock_WithAvailableStock_CanReserveStockSuccessfully()
    {
        // Arrange
        var product = new Product("Tastiera meccanica", 89.99m, 50);
        var quantityToReserve = 10;

        // Act
        product.ReserveStock(quantityToReserve);

        // Assert
        product.StockQuantity.Should().Be(40);
    }

    [Fact]
    public void ReserveStock_WithInsufficientStock_CannotReserveMoreThanAvailable()
    {
        // Arrange
        var product = new Product("Tastiera meccanica", 89.99m, 50);
        var quantityToReserve = 60;

        // Act
        Action act = () => product.ReserveStock(quantityToReserve);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ReleaseStock_WithAvailableStock_CanReleaseStockSuccessfully()
    {
        // Arrange
        var product = new Product("Tastiera meccanica", 89.99m, 50);
        var quantityToRelease = 10;

        // Act
        product.ReleaseStock(quantityToRelease);

        // Assert
        product.StockQuantity.Should().Be(60);
    }

    [Fact]
    public void ReleaseStock_WithInvalidQuantity_CannotReleaseStockSuccessfully()
    {
        // Arrange
        var product = new Product("Tastiera meccanica", 89.99m, 50);
        var quantityToRelease = -5;

        // Act
        Action act = () => product.ReleaseStock(quantityToRelease);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}