using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payment.API.Dtos;
using Payment.API.Features.PaymentFeatures.Commands;

namespace Payment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequestDto paymentRequest)
    {
        var command = new ProcessPaymentCommand
        {
            OrderId = paymentRequest.OrderId,
            Amount = paymentRequest.Amount
        };

        var result = await mediator.Send(command);

        // The controller now inspects the result from the handler
        // to decide which HTTP status code to return.
        if (result.Status == "Success")
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}