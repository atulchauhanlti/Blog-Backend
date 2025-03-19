using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models.PostDtos;

public class AddCommentRequest
{
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Content { get; set; }
}
