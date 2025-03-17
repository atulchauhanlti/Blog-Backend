using System;

namespace backend.Models.PostDtos;

public class CommentResponse
{
    public string Content { get; set; }
    public string Author { get; set; } // Assuming the `Author` is the username or name of the user who made the comment
}
