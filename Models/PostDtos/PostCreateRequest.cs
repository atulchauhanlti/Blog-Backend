using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Models.PostDtos;

public class PostCreateRequest
{
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public string TagIds { get; set; }
}
