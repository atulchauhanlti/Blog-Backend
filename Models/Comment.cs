using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Comment
{
    public int Id { get; set; }

    [Required]
    public string Content { get; set; }

    [Required]
    public DateTime CommentedAt { get; set; }

    [Required]
    public int UserId { get; set; }

    public User User { get; set; }

    [Required]
    public int PostId { get; set; }

    public Post Post { get; set; }
}

