using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    [StringLength(20)]
    public string Role { get; set; } // Example: "Admin" or "User"

    public ICollection<Post> Posts { get; set; }
    public ICollection<Comment> Comments { get; set; }
}

