using MediatR;
using Microsoft.IdentityModel.Tokens;
using Security.API.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Security.API.Features.AuthFeatures.Commands;

public class LoginCommandHandler(IConfiguration configuration) : IRequestHandler<LoginCommand, LoginResponseDto>
{
    public Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if (request.Username != "test" || request.Password != "password")
        {
            return Task.FromResult<LoginResponseDto>(null!);
        }
        var tokenString = GenerateJwtToken(request.Username!);
        return Task.FromResult(new LoginResponseDto { Token = tokenString });
    }

    private string GenerateJwtToken(string username)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, username),
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