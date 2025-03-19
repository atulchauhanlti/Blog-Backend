using System;
using System.Collections.Generic;
using System.Linq;
using backend.Helpers;
using backend.Interface;
using backend.Models;
using backend.Models.PostDtos;

namespace backend.Services;

public class PostService
{
    private readonly IPostRepository _postRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITagRepository _tagRepository;

    public PostService(IPostRepository postRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
    {
        _postRepository = postRepository;
         _categoryRepository = categoryRepository;
         _tagRepository = tagRepository;
    }

    public void AddPost(PostCreateRequest request, int userId, string imageUrl)
    {
        var category = _categoryRepository.GetCategoryById(request.CategoryId);
        if (category == null) throw new Exception("Category does not exist.");

        var tagIdList = request.TagIds
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToList();

        var post = new Post
        {
            Title = request.Title,
            Content = request.Content,
            PublishedAt = DateTime.UtcNow,
            UserId = userId,
            CategoryId = request.CategoryId,
            Slug = CommandHelper.GenerateSlug(request.Title),
            ImageUrl = imageUrl, 
            PostTags = tagIdList.Select(tagId => new PostTag { TagId = tagId }).ToList()
        };

        _postRepository.AddPost(post);
    }
    public void UpdatePost(int id, PostUpdateRequest request, string imageUrl = null)
    {
        var post = _postRepository.GetPostById(id); 
        if (post == null) throw new Exception("Post not found!");

        var tagIdList = request.TagIds
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToList();

        post.Title = request.Title;
        post.Content = request.Content;
        post.CategoryId = request.CategoryId;

        if (!string.IsNullOrEmpty(imageUrl))
        {
            post.ImageUrl = imageUrl; 
        }

        post.PostTags = tagIdList.Select(tagId => new PostTag { TagId = tagId, PostId = post.Id }).ToList();

        _postRepository.UpdatePost(post);
    }
    public void DeletePost(int id)
    {
        _postRepository.DeletePost(id);
    }

    public IEnumerable<PostResponse> GetPostsByCategory(int categoryId)
    {
        return _postRepository.GetPostsByCategory(categoryId)
                              .Select(post => new PostResponse
                                {
                                    Id = post.Id,
                                    Title = post.Title,
                                    Content = post.Content,
                                    PublishedAt = post.PublishedAt,
                                    Slug = post.Slug,
                                    ImageUrl = post.ImageUrl,
                                    CategoryName = post.Category?.Name ?? "Unknown",
                                    Tags = post.PostTags?.Select(pt => pt.Tag?.Name).ToList() ?? new List<string>()
                                });
    }

    public PostResponse GetPostBySlug(string slug)
    {
        var post = _postRepository.GetPostBySlug(slug);
        if (post == null) throw new Exception("Post not found!");

        return new PostResponse
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            PublishedAt = post.PublishedAt,
            Slug = post.Slug,
            ImageUrl = post.ImageUrl,
            CategoryName = post.Category.Name,
            Tags = post.PostTags.Select(pt => pt.Tag.Name).ToList(),
            Comments = post.Comments?.Select(c => new CommentResponse
            {
                Author = c.Name,
                Content = c.Content,
                CommentedAt = c.CommentedAt
            }).ToList() ?? new List<CommentResponse>()
        };
    }

    public PostResponse GetPostById(int id)
    {
        var post = _postRepository.GetPostById(id);
        if (post == null)
        {
            throw new Exception($"Post with ID {id} not found.");
        }

        return new PostResponse
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            PublishedAt = post.PublishedAt,
            Slug = post.Slug,
            ImageUrl = post.ImageUrl,
            CategoryName = post.Category?.Name, 
            Tags = post.PostTags?.Select(pt => pt.Tag?.Name).ToList() ?? new List<string>()
        };
    }

    public IEnumerable<PostResponse> GetPostsByTag(int tagId)
    {
        return _postRepository.GetPostsByTag(tagId)
                            .Select(post => new PostResponse
                            {
                                Id = post.Id,
                                Title = post.Title,
                                Content = post.Content,
                                PublishedAt = post.PublishedAt,
                                Slug = post.Slug,
                                ImageUrl = post.ImageUrl,
                                CategoryName = post.Category.Name,
                                Tags = post.PostTags.Select(pt => pt.Tag.Name).ToList()
                            }).ToList();
    }

    public IEnumerable<PostResponse> GetPostsByUser(int userId)
    {
        return _postRepository.GetPostsByUser(userId)
                            .Select(post => new PostResponse
                            {
                                Id = post.Id,
                                Title = post.Title,
                                Content = post.Content,
                                PublishedAt = post.PublishedAt,
                                Slug = post.Slug,
                                ImageUrl = post.ImageUrl,
                                CategoryName = post.Category.Name,
                                Tags = post.PostTags.Select(pt => pt.Tag.Name).ToList()
                            }).ToList();
    }

   public void AddCommentOnPost(int postId, string name, string email, string content)
    {
        var post = _postRepository.GetPostById(postId);

        if (post == null)
        {
            throw new ArgumentException("Post not found");
        }

        var comment = new Comment
        {
            Name = name,
            Email = email,
            Content = content,
            CommentedAt = DateTime.Now,
            PostId = postId
        };

        _postRepository.AddComment(comment);
    }

    public IEnumerable<PostResponse> SearchPosts(string query)
    {
        return _postRepository.SearchPosts(query)
                            .Select(post => new PostResponse
                            {
                                Title = post.Title,
                                Content = post.Content,
                                PublishedAt = post.PublishedAt,
                                Slug = post.Slug,
                                CategoryName = post.Category.Name,
                                Tags = post.PostTags.Select(pt => pt.Tag.Name).ToList()
                            }).ToList();
    }

    public IEnumerable<PostResponse> GetAllPosts()
    {
        return _postRepository.GetAllPosts()
                            .Select(post => new PostResponse
                            {
                                Id = post.Id,
                                Title = post.Title,
                                Content = post.Content,
                                PublishedAt = post.PublishedAt,
                                Slug = post.Slug,
                                CategoryName = post.Category.Name,
                                ImageUrl = post.ImageUrl,
                                Tags = post.PostTags.Select(pt => pt.Tag.Name).ToList()
                            }).ToList();
    }
}
