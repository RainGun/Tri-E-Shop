using MediatR;
using Microsoft.IdentityModel.Tokens;
using Security.API.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Security.API.Features.AuthFeatures.Commands;

public class LoginCommandHandler(IConfiguration configuration)
    : IRequestHandler<LoginCommand, LoginResponseDto>
{
    public Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Read the demo user's data from the configuration file.
        var demoUser = configuration.GetSection("DemoUser");

        // 2. Validate credentials against the configuration file.
        if (request.Username != demoUser["Username"] || request.Password != demoUser["Password"])
        {
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

        // 4. Create the claims, now including the user's role.
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(ClaimTypes.Role, role), // The user's role is now part of the token.
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