using System;
using System.Collections.Generic;
using System.Linq;
using backend.Data;
using backend.Interface;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class PostRepository : IPostRepository
{
    private readonly ApplicationDbContext _context;

    public PostRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Post GetPostBySlug(string slug)
    {
        return _context.Posts
                       .Include(p => p.Category)
                       .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
                       .Include(p => p.Comments)
                       .SingleOrDefault(p => p.Slug == slug);
    }

    public IEnumerable<Post> GetPostsByCategory(int categoryId)
    {
        return _context.Posts
                       .Where(p => p.CategoryId == categoryId)
                       .ToList();
    }

    public IEnumerable<Post> GetPostsByTag(int tagId)
    {
        return _context.PostTags
                       .Where(pt => pt.TagId == tagId)
                       .Select(pt => pt.Post)
                       .ToList();
    }

    public IEnumerable<Post> GetPostsByUser(int userId)
    {
        return _context.Posts
                       .Where(p => p.UserId == userId)
                       .ToList();
    }

    public IEnumerable<Post> SearchPosts(string query)
    {
        return _context.Posts
                       .Where(p => p.Title.Contains(query) || p.Content.Contains(query))
                       .ToList();
    }

    public IEnumerable<Post> GetAllPosts()
    {
        return _context.Posts
                    .Include(p => p.Category)
                    .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
                    .Include(p => p.Comments)
                    .ToList();
    }

    public void AddPost(Post post)
    {
        _context.Posts.Add(post);
        _context.SaveChanges();
    }

    public Post GetPostById(int id)
    {
        var post = _context.Posts.FirstOrDefault(p => p.Id == id);
        if (post == null)
        {
            Console.WriteLine($"Post with ID {id} not found.");
            // Optionally log the error or return null (current behavior).
        }
        return post;
    }

    public void UpdatePost(Post post)
    {
        _context.Posts.Update(post);
        _context.SaveChanges();
    }

    public void DeletePost(int id)
    {
        var post = _context.Posts.Find(id);
        if (post != null)
        {
            _context.Posts.Remove(post);
            _context.SaveChanges();
        }
    }
}
