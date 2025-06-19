using System.ComponentModel.DataAnnotations; // Add this using statement

namespace Security.API.Dtos;

public class LoginRequestDto
{
    [Required(ErrorMessage = "Username is required")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}

// LoginResponseDto remains the same
public class LoginResponseDto
{
    public string? Token { get; set; }
}