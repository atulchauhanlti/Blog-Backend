using System;
using backend.Models;
using backend.Models.UserDtos;

namespace backend.Interface;

public interface IUserService
{
    User GetUserById(string userId);
    User UpdateUser(string userId, UpdateUserRequest request);
}
