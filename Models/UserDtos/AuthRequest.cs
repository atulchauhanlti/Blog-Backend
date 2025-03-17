using System;

namespace backend.Models.UserDtos;

public class AuthRequest
{
    public required string UsernameOrEmail { get; set; }
    public required string Password { get; set; }
}
