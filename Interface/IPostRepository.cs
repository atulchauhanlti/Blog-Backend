using System;
using System.Collections.Generic;
using backend.Models;

namespace backend.Interface;

public interface IPostRepository
{
    Post GetPostBySlug(string slug);
    IEnumerable<Post> GetPostsByCategory(int categoryId);
    IEnumerable<Post> GetPostsByTag(int tagId);
    IEnumerable<Post> GetPostsByUser(int userId);
    IEnumerable<Post> SearchPosts(string query);
    void AddPost(Post post);
    void UpdatePost(Post post);
    void DeletePost(int id);
}
