using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Query
{
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(500)]
    public string Message { get; set; }

    [Required]
    public DateTime SentAt { get; set; }
}

