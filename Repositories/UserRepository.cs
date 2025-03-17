using System;
using System.Linq;
using backend.Data;
using backend.Interface;
using backend.Models;

namespace backend.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context; 

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public User GetByUsername(string username)
    {
        return _context.Users.SingleOrDefault(u => u.Username == username);
    }

    public User GetByUsernameOrEmail(string usernameOrEmail)
    {
        return _context.Users
            .SingleOrDefault(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }
}
