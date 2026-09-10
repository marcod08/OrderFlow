using Ordering.Service.Domain;
using FluentAssertions;

namespace Ordering.Service.UnitTests;

public class OrderTests 
{
    [Fact]
    public void Constructor_WithValidData_CreatesOrderSuccessfully()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var quantity = 5;

        // Act
        var order = new Order(productId, quantity);

        // Assert
        order.Id.Should().NotBe(Guid.Empty);
        order.ProductId.Should().Be(productId);
        order.Quantity.Should().Be(quantity);
        order.Status.Should().Be(OrderStatus.Pending);
        order.TotalPrice.Should().BeNull();
        order.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Constructor_WithInvalidQuantity_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var quantity = 0;

        //Act
        Action act = () => new Order(productId, quantity);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void MarkStockReserved_FromPendingStatus_ChangesStatusToStockReserved()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(), 5);

        // Act
        order.MarkStockReserved();

        // Assert
        order.Status.Should().Be(OrderStatus.StockReserved);
    }

    [Fact]
    public void MarkStockReserved_FromNonPendingStatus_ThrowsInvalidOperationException()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(), 5);
        order.MarkStockReserved(); 

        // Act
        Action act = () => order.MarkStockReserved();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void MarkPaymentProcessed_FromStockReservedStatus_ChangesStatusToPaymentProcessed()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(), 5);
        order.MarkStockReserved(); 
        
        //Act
        order.MarkPaymentProcessed();

        // Assert
        order.Status.Should().Be(OrderStatus.PaymentProcessed);
    }

    [Fact]
    public void MarkPaymentProcessed_FromNonStockReservedStatus_ThrowsInvalidOperationException()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(), 5);

        // Act
        Action act = () => order.MarkPaymentProcessed();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void MarkConfirmed_FromPaymentProcessedStatus_ChangesStatusToConfirmed()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(), 5);
        order.MarkStockReserved(); 
        order.MarkPaymentProcessed(); 

        // Act
        order.MarkConfirmed();

        // Assert
        order.Status.Should().Be(OrderStatus.Confirmed);
    }

    [Fact]
    public void MarkConfirmed_FromNonPaymentProcessedStatus_ThrowsInvalidOperationException()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(), 5);

        // Act
        Action act = () => order.MarkConfirmed();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void MarkCancelled_FromNonTerminalStatus_ChangesStatusToCancelled()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(), 5);
        order.MarkStockReserved();

        // Act
        order.MarkCancelled();

        // Assert
        order.Status.Should().Be(OrderStatus.Cancelled);
    }

    [Fact]
    public void MarkCancelled_FromConfirmedStatus_ThrowsInvalidOperationException()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(), 5);
        order.MarkStockReserved();
        order.MarkPaymentProcessed();
        order.MarkConfirmed();

        // Act
        Action act = () => order.MarkCancelled();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void SetTotalPrice_WithValidPrice_SetsTotalPriceSuccessfully()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(), 5);
        var totalPrice = 100.00m;

        // Act
        order.SetTotalPrice(totalPrice);

        // Assert
        order.TotalPrice.Should().Be(totalPrice);
    }

    [Fact]
    public void SetTotalPrice_WithNegativePrice_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(), 5);
        var totalPrice = -50.00m;

        // Act
        Action act = () => order.SetTotalPrice(totalPrice);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}