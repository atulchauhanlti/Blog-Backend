using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class PostTag
{
    [Required]
    public int PostId { get; set; }

    public Post Post { get; set; }

    [Required]
    public int TagId { get; set; }

    public Tag Tag { get; set; }
}

