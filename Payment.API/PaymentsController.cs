using Microsoft.AspNetCore.Mvc;
using Payment.API.Dtos;

namespace Payment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// We use a primary constructor to inject both the logger and configuration.
public class PaymentsController(ILogger<PaymentsController> logger, IConfiguration configuration) : ControllerBase
{
    // This endpoint simulates processing a payment.
    [HttpPost]
    public IActionResult ProcessPayment([FromBody] PaymentRequestDto paymentRequest)
    {
        logger.LogInformation("Processing payment for OrderId: {OrderId}", paymentRequest.OrderId);

        // Read the failure threshold from appsettings.json
        var failureThreshold = configuration.GetValue<decimal>("PaymentSettings:FailureThreshold");

        // The simulation logic now uses the configured value.
        bool isSuccessful = paymentRequest.Amount <= failureThreshold;

        if (isSuccessful)
        {
            var response = new PaymentResponseDto
            {
                PaymentId = Guid.NewGuid(),
                Status = "Success",
                Message = $"Payment for order {paymentRequest.OrderId} was successfully processed."
            };
            logger.LogInformation("Payment successful for OrderId: {OrderId}", paymentRequest.OrderId);
            return Ok(response);
        }
        else
        {
            var response = new PaymentResponseDto
            {
                PaymentId = Guid.Empty,
                Status = "Failed",
                Message = $"Payment for order {paymentRequest.OrderId} failed due to amount exceeding limit of {failureThreshold}."
            };
            logger.LogWarning("Payment failed for OrderId: {OrderId}", paymentRequest.OrderId);
            return BadRequest(response);
        }
    }
}