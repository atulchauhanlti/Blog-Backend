using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Tag
{
    public int Id { get; set; }

    [Required]
    [StringLength(30)]
    public string Name { get; set; }

    public ICollection<PostTag>? PostTags { get; set; } = new List<PostTag>();
}

