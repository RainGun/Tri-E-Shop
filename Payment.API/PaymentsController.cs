using Microsoft.AspNetCore.Mvc;
using Payment.API.Dtos;

namespace Payment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(ILogger<PaymentsController> logger)
        {
            _logger = logger;
        }

        // This endpoint simulates processing a payment.
        [HttpPost]
        public IActionResult ProcessPayment([FromBody] PaymentRequestDto paymentRequest)
        {
            _logger.LogInformation("Processing payment for OrderId: {OrderId}", paymentRequest.OrderId);
            bool isSuccessful = paymentRequest.Amount <= 1000; // if higher than 1000, fail the payment.

            if (isSuccessful)
            {
                var response = new PaymentResponseDto
                {
                    PaymentId = Guid.NewGuid(),
                    Status = "Success",
                    Message = $"Payment for order {paymentRequest.OrderId} was successfully processed."
                };
                _logger.LogInformation("Payment successful for OrderId: {OrderId}", paymentRequest.OrderId);
                return Ok(response); // Return 200 OK for success
            }
            else
            {
                var response = new PaymentResponseDto
                {
                    PaymentId = Guid.Empty,
                    Status = "Failed",
                    Message = $"Payment for order {paymentRequest.OrderId} failed due to amount exceeding limit."
                };
                _logger.LogWarning("Payment failed for OrderId: {OrderId}", paymentRequest.OrderId);
                return BadRequest(response); // Return 400 Bad Request for failure
            }
        }
    }
}