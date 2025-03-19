using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Comment
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } // Add Name field

    [Required]
    [EmailAddress]
    public string Email { get; set; } // Add Email field

    [Required]
    public string Content { get; set; }

    [Required]
    public DateTime CommentedAt { get; set; }

    [Required]
    public int PostId { get; set; }

    public Post Post { get; set; }
}

