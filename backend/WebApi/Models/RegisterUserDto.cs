using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models;

public class RegisterUserDto
{
    [Required, EmailAddress] public string Email { get; init; } = null!;
    [Required] public string Password { get; init; } = null!;
    [Required] public string FirstName { get; init; } = null!;
    [Required] public string LastName { get; init; } = null!;
}