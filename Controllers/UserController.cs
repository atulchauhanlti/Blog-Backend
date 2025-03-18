using System;
using System.Collections.Generic;
using backend.Interface;
using backend.Models;
using backend.Models.UserDtos;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Authorize] // Require authorization
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/User/Profile
        [HttpGet("profile")]
        public IActionResult GetUserProfile()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return Unauthorized("User ID is missing in the token.");
            }

            var userId = userIdClaim.Value;

            var user = _userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user); // Return user profile
        }

        // PUT: api/User/Update
        [HttpPut("update")]
        public IActionResult UpdateUser([FromBody] UpdateUserRequest request)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return Unauthorized("User ID is missing in the token.");
            }

            var userId = userIdClaim.Value;

            try
            {
                var updatedUser = _userService.UpdateUser(userId, request);
                return Ok(updatedUser); // Return updated user profile
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "User not found" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to update user", error = ex.Message });
            }
        }
    }
}
