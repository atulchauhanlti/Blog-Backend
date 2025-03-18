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
        // Validate CategoryId
        var category = _categoryRepository.GetCategoryById(request.CategoryId);
        if (category == null) throw new Exception("Category does not exist.");

        // Parse TagIds from comma-separated string
        var tagIdList = request.TagIds
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToList();

        // Validate TagIds (Optional: Check if these tag IDs exist in the database)
        // var tags = _tagRepository.GetTagsByIds(tagIdList);

        // Create the Post
        var post = new Post
        {
            Title = request.Title,
            Content = request.Content,
            PublishedAt = DateTime.UtcNow,
            UserId = userId,
            CategoryId = request.CategoryId,
            Slug = CommandHelper.GenerateSlug(request.Title),
            ImageUrl = imageUrl, // Handle the image URL
            PostTags = tagIdList.Select(tagId => new PostTag { TagId = tagId }).ToList()
        };

        _postRepository.AddPost(post);
    }
    public void UpdatePost(int id, PostUpdateRequest request, string imageUrl = null)
    {
        // Retrieve the post by its id
        var post = _postRepository.GetPostById(id); // Use the id to fetch the post
        if (post == null) throw new Exception("Post not found!");

        // Parse TagIds from a comma-separated string in the request
        var tagIdList = request.TagIds
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToList();

        // Update post fields
        post.Title = request.Title;
        post.Content = request.Content;
        post.CategoryId = request.CategoryId;

        if (!string.IsNullOrEmpty(imageUrl))
        {
            post.ImageUrl = imageUrl; // Update image URL if a new image is provided
        }

        // Update post tags
        post.PostTags = tagIdList.Select(tagId => new PostTag { TagId = tagId, PostId = post.Id }).ToList();

        // Save the changes via the repository
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
                                  Title = post.Title,
                                  Content = post.Content,
                                  PublishedAt = post.PublishedAt,
                                  Slug = post.Slug,
                                  CategoryName = post.Category.Name,
                                  Tags = post.PostTags.Select(pt => pt.Tag.Name).ToList(),
                                  Comments = post.Comments.Select(c => new CommentResponse
                                  {
                                      Content = c.Content,
                                      Author = c.Author.Username
                                  }).ToList()
                              }).ToList();
    }

    public PostResponse GetPostBySlug(string slug)
    {
        var post = _postRepository.GetPostBySlug(slug);
        if (post == null) throw new Exception("Post not found!");

        return new PostResponse
        {
            Title = post.Title,
            Content = post.Content,
            PublishedAt = post.PublishedAt,
            Slug = post.Slug,
            CategoryName = post.Category.Name,
            Tags = post.PostTags.Select(pt => pt.Tag.Name).ToList(),
            Comments = post.Comments.Select(c => new CommentResponse
            {
                Content = c.Content,
                Author = c.Author.Username
            }).ToList()
        };
    }

    public IEnumerable<PostResponse> GetPostsByTag(int tagId)
    {
        return _postRepository.GetPostsByTag(tagId)
                            .Select(post => new PostResponse
                            {
                                Title = post.Title,
                                Content = post.Content,
                                PublishedAt = post.PublishedAt,
                                Slug = post.Slug,
                                CategoryName = post.Category.Name,
                                Tags = post.PostTags.Select(pt => pt.Tag.Name).ToList(),
                                Comments = post.Comments.Select(c => new CommentResponse
                                {
                                    Content = c.Content,
                                    Author = c.Author.Username
                                }).ToList()
                            }).ToList();
    }

    public IEnumerable<PostResponse> GetPostsByUser(int userId)
    {
        return _postRepository.GetPostsByUser(userId)
                            .Select(post => new PostResponse
                            {
                                Title = post.Title,
                                Content = post.Content,
                                PublishedAt = post.PublishedAt,
                                Slug = post.Slug,
                                CategoryName = post.Category.Name,
                                Tags = post.PostTags.Select(pt => pt.Tag.Name).ToList(),
                                Comments = post.Comments.Select(c => new CommentResponse
                                {
                                    Content = c.Content,
                                    Author = c.Author.Username
                                }).ToList()
                            }).ToList();
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
                                Tags = post.PostTags.Select(pt => pt.Tag.Name).ToList(),
                                Comments = post.Comments.Select(c => new CommentResponse
                                {
                                    Content = c.Content,
                                    Author = c.Author.Username
                                }).ToList()
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
                                Tags = post.PostTags.Select(pt => pt.Tag.Name).ToList(),
                                Comments = post.Comments.Select(c => new CommentResponse
                                {
                                    Content = c.Content,
                                    Author = c.Author.Username
                                }).ToList()
                            }).ToList();
    }
}
