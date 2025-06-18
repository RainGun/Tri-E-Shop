using MediatR;
using Security.API.Dtos;

namespace Security.API.Features.AuthFeatures.Commands;

// This command carries the user's credentials.
// It implements IRequest<LoginResponseDto>, meaning it expects a DTO with a token back.
public class LoginCommand : IRequest<LoginResponseDto>
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}