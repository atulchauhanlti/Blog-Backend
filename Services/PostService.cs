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

    public void AddPost(PostCreateRequest request, int userId)
    {
        // Validate CategoryId
        var category = _categoryRepository.GetCategoryById(request.CategoryId);
        if (category == null) throw new Exception("Category does not exist.");

        // Validate TagIds
        // var tags = _tagRepository.GetTagsByIds(request.TagIds);
        // if (tags.Count != request.TagIds.Count)
        // {
        //     throw new Exception("One or more tags do not exist.");
        // }

        // Create the Post
        var post = new Post
        {
            Title = request.Title,
            Content = request.Content,
            PublishedAt = DateTime.UtcNow,
            UserId = userId,
            CategoryId = request.CategoryId,
            Slug = CommandHelper.GenerateSlug(request.Title),
            PostTags = request.TagIds.Select(tagId => new PostTag { TagId = tagId }).ToList()
        };

        _postRepository.AddPost(post);
    }

    public void UpdatePost(PostUpdateRequest request)
    {
        var post = _postRepository.GetPostBySlug(CommandHelper.GenerateSlug(request.Title));
        if (post == null) throw new Exception("Post not found!");

        post.Title = request.Title;
        post.Content = request.Content;
        post.CategoryId = request.CategoryId;
        post.PostTags = request.TagIds.Select(tagId => new PostTag { TagId = tagId, PostId = post.Id }).ToList();

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
}
