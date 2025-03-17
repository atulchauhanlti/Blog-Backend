using System;
using backend.Models;

namespace backend.Interface;

public interface IUserRepository
{
    User GetByUsername(string username);
    User GetByUsernameOrEmail(string usernameOrEmail); // New method
    void Add(User user);
}
