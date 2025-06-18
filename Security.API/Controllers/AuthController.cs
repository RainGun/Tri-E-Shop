using MediatR;
using Microsoft.AspNetCore.Mvc;
using Security.API.Dtos;
using Security.API.Features.AuthFeatures.Commands;

namespace Security.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        // 1. Create the command object with the data from the request.
        var command = new LoginCommand
        {
            Username = loginRequest.Username,
            Password = loginRequest.Password
        };

        // 2. Send the command to MediatR. It will find the corresponding handler.
        var result = await mediator.Send(command);

        // 3. Check the result from the handler.
        // If the handler returns null, it means authentication failed.
        if (result is null)
        {
            return Unauthorized("Invalid credentials");
        }

        // If successful, return 200 OK with the token.
        return Ok(result);
    }
}