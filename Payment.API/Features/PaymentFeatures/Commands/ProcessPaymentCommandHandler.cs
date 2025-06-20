using MediatR;
using Payment.API.Dtos;

namespace Payment.API.Features.PaymentFeatures.Commands;

public class ProcessPaymentCommandHandler(ILogger<ProcessPaymentCommandHandler> logger, IConfiguration configuration)
    : IRequestHandler<ProcessPaymentCommand, PaymentResponseDto>
{
    public Task<PaymentResponseDto> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing payment for OrderId: {OrderId}", request.OrderId);

        var failureThreshold = configuration.GetValue<decimal>("PaymentSettings:FailureThreshold");

        // The simulation logic now lives inside the handler.
        bool isSuccessful = request.Amount <= failureThreshold;

        if (isSuccessful)
        {
            var response = new PaymentResponseDto
            {
                PaymentId = Guid.NewGuid(),
                Status = "Success",
                Message = $"Payment for order {request.OrderId} was successfully processed."
            };
            logger.LogInformation("Payment successful for OrderId: {OrderId}", request.OrderId);
            return Task.FromResult(response);
        }
        else
        {
            var response = new PaymentResponseDto
            {
                PaymentId = Guid.Empty,
                Status = "Failed",
                Message = $"Payment for order {request.OrderId} failed due to amount exceeding limit of {failureThreshold}."
            };
            logger.LogWarning("Payment failed for OrderId: {OrderId}", request.OrderId);
            // In CQRS, we typically don't return different status codes from the handler.
            // We return a DTO that indicates success or failure. The controller decides the HTTP status code.
            return Task.FromResult(response);
        }
    }
}