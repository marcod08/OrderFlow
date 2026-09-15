using MediatR;

namespace Billing.Service.Application.Payments.ProcessPayment;

public record ProcessPaymentCommand(Guid OrderId, decimal Amount) : IRequest<bool>;