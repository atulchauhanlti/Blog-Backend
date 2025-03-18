using System;
using System.Collections.Generic;
using System.Linq;
using backend.Data;
using backend.Interface;
using backend.Models;
using backend.Models.UserDtos;

namespace backend.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public User GetUserById(string userId)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found");
        }

        return user;
    }

   public User UpdateUser(string userId, UpdateUserRequest request)
    {
        // Find the user in the database
        var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found");
        }

        // Update only the fields provided in the request
        if (!string.IsNullOrEmpty(request.Email))
        {
            user.Email = request.Email;
        }

        if (!string.IsNullOrEmpty(request.FirstName))
        {
            user.FirstName = request.FirstName;
        }

        if (!string.IsNullOrEmpty(request.LastName))
        {
            user.LastName = request.LastName;
        }

        if (!string.IsNullOrEmpty(request.Password))
        {
            // Hash the password before storing it
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        }

        // Save changes to the database
        _context.SaveChanges();

        return user;
    }
}
