using System;
using System.Collections.Generic;

namespace backend.Models.PostDtos;

public class PostResponse
{
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PublishedAt { get; set; }
    public string Slug { get; set; }
    public string CategoryName { get; set; }
    public List<string> Tags { get; set; } // List of tag names
    public List<CommentResponse> Comments { get; set; }
}
