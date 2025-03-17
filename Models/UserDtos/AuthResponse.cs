using System;

namespace backend.Models.UserDtos;

public class AuthResponse
{
    public required string Token { get; set; }
    public required string Username { get; set; }
    public required string Role { get; set; }
}
