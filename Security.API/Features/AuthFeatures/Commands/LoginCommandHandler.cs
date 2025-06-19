using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging; // Add this using for ILogger
using Microsoft.IdentityModel.Tokens;
using Security.API.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Security.API.Features.AuthFeatures.Commands;

// CHANGE 1: Inject ILogger into the primary constructor
public class LoginCommandHandler(IConfiguration configuration, ILogger<LoginCommandHandler> logger)
    : IRequestHandler<LoginCommand, LoginResponseDto>
{
    public Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Read the demo user's data from the configuration file.
        var demoUser = configuration.GetSection("DemoUser");

        if (request.Username != demoUser["Username"] || request.Password != demoUser["Password"])
        {
            //2. Log a warning for the failed login attempt.
            logger.LogWarning("Failed login attempt for user: {Username}", request.Username);

            return Task.FromResult<LoginResponseDto>(null!);
        }

        // 3. Pass the username and role to the token generator.
        var tokenString = GenerateJwtToken(request.Username!, demoUser["Role"]!);
        var response = new LoginResponseDto { Token = tokenString };

        return Task.FromResult(response);
    }

    private string GenerateJwtToken(string username, string role)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // 4.Create the claims, now including the user's role.
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}