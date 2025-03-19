using System;

namespace backend.Models.PostDtos;

public class CommentResponse
{
    public string Content { get; set; }
    public string Author { get; set; } 

    public DateTime CommentedAt {get; set;}
}
