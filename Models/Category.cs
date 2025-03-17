using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Category
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public ICollection<Post> Posts { get; set; }
}

