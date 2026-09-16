using Billing.Service.Domain;
using Billing.Service.Infrastructure.Repositories;
using FluentAssertions;

namespace Billing.Service.IntegrationTests;

public class PaymentRepositoryTests(PostgresTestFixture fixture) : IClassFixture<PostgresTestFixture>
{
    [Fact]
    public async Task AddAsync_ThenSaveChanges_PersistspaymentSuccessfully()
    {
        // Arrange
        var repository = new PaymentRepository(fixture.DbContext);
        var payment = new Payment(Guid.NewGuid(), 19.99m);

        // act
        await repository.AddAsync(payment,CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        // Assert
        var savedPayment = await fixture.DbContext.Payments.FindAsync(payment.Id);

        savedPayment.Should().NotBeNull();
        savedPayment!.OrderId.Should().Be(payment.OrderId);
        savedPayment.Amount.Should().Be(payment.Amount);
        savedPayment.Status.Should().Be(PaymentStatus.Pending);
    }
}