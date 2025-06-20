using MediatR;
using Payment.API.Dtos;

namespace Payment.API.Features.PaymentFeatures.Commands;

// This command carries the data for the payment request.
// It expects a PaymentResponseDto back.
public class ProcessPaymentCommand : IRequest<PaymentResponseDto>
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
}