using Billing.Service.Domain;
using FluentAssertions;

namespace Billing.Service.UnitTests;

public class PaymentTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesPaymentSuccessfully()
    {
        // arrange
        var orderId = Guid.NewGuid();
        var amount = 19.99m;

        // act
        var payment = new Payment(orderId,amount);

        // assert
        payment.Id.Should().NotBe(Guid.Empty);
        payment.OrderId.Should().Be(orderId);
        payment.Amount.Should().Be(amount);
        payment.Status.Should().Be(PaymentStatus.Pending);
        payment.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Constructor_WithInvalidAmount_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var amount = 0;

        // Act
        Action act = () => new Payment(orderId, amount);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();

    }

    [Fact]
    public void MarkCompleted_FromNonPendingStatus_ThrowsInvalidOperationException()
    {
        // Arrange
        var payment = new Payment(Guid.NewGuid(), 19.99m);
        payment.MarkCompleted(); 

        // Act
        Action act = () => payment.MarkCompleted();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void MarkCompleted_FromPendingStatus_ChangesStatusToCompleted()
    {   
        // Arrange
        var payment = new Payment(Guid.NewGuid(), 19.99m);

        // Act
        payment.MarkCompleted();

        // Assert
        payment.Status.Should().Be(PaymentStatus.Completed);
    }

    [Fact]
    public void MarkFailed_FromPendingStatus_ChangesStatusToFailed()
    {
        // arrange
        var payment = new Payment(Guid.NewGuid(), 19.99m);

        // Act
        payment.MarkFailed();

        // Assert
        payment.Status.Should().Be(PaymentStatus.Failed);
    }

    [Fact]
    public void MarkFailed_FromNonPendingStatus_ThrowsInvalidOperationException()
    {
        // arrange
        var payment = new Payment(Guid.NewGuid(), 19.99m);
        payment.MarkCompleted();

        // Act
        Action act = () => payment.MarkFailed();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}