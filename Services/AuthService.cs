using System;
using System.Security.Cryptography;
using System.Text;
using backend.Helpers;
using backend.Interface;
using backend.Models;
using backend.Models.UserDtos;

namespace backend.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtHelper _jwtHelper;

    public AuthService(IUserRepository userRepository, JwtHelper jwtHelper)
    {
        _userRepository = userRepository;
        _jwtHelper = jwtHelper;
    }

    public AuthResponse Register(RegistrationRequest request)
    {
        var hashedPassword = HashPassword(request.Password);

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordHash = hashedPassword,
            Role = "User"
        };

        _userRepository.Add(user);

        var token = _jwtHelper.GenerateToken(user);

        return new AuthResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.Role
        };
    }

    public AuthResponse Login(string usernameOrEmail, string password)
    {
        var user = _userRepository.GetByUsernameOrEmail(usernameOrEmail);

        if (user == null || !VerifyPassword(password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var token = _jwtHelper.GenerateToken(user);

        return new AuthResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.Role
        };
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        return HashPassword(password) == hashedPassword;
    }
}
