using System;
using System.Collections.Generic;

namespace backend.Models.PostDtos;

public class PostResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PublishedAt { get; set; }
    public string Slug { get; set; }
    public string CategoryName { get; set; }
    public List<string> Tags { get; set; } 
    public List<CommentResponse> Comments { get; set; }
    public string ImageUrl { get; set; }
}
