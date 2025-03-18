using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Post
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    [Required]
    public DateTime PublishedAt { get; set; }

    [Required]
    public int UserId { get; set; }

    public User User { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public Category Category { get; set; }

    // Add a unique slug for frontend references
    [Required]
    [StringLength(150)]
    public string Slug { get; set; }

    // Image URL or File Path
    [StringLength(500)]
    public string ImageUrl { get; set; } // This stores the image path or URL

    public ICollection<PostTag> PostTags { get; set; }
    public ICollection<Comment> Comments { get; set; }
}

