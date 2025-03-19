using System;
using System.Linq;
using backend.Models.PostDtos;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Collections.Generic;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly CategoryService _categoryService;

        public PostsController(PostService postService, CategoryService categoryService)
        {
            _postService = postService;
            _categoryService = categoryService;
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult AddPost([FromForm] PostCreateRequest request, [FromForm] IFormFile imageFile)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return Unauthorized("User ID is missing in the token.");
            }

            var userId = int.Parse(userIdClaim.Value);

            string imageUrl = null;
            if (imageFile != null && imageFile.Length > 0)
            {
                var filePath = Path.Combine("uploads", imageFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }
                imageUrl = filePath; 
            }

            _postService.AddPost(request, userId, imageUrl);
            return Ok("Post created successfully");
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public IActionResult UpdatePost(int id, [FromForm] PostUpdateRequest request, [FromForm] IFormFile? imageFile)
        {
            string imageUrl = null;

            if (imageFile != null && imageFile.Length > 0)
            {
                var filePath = Path.Combine("uploads", imageFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }
                imageUrl = filePath; 
            }

            _postService.UpdatePost(id, request, imageUrl);
            return NoContent();
        }


        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeletePost(int id)
        {
            _postService.DeletePost(id);
            return NoContent();
        }


       [HttpGet("category/{categoryName}")]
        public IActionResult GetPostsByCategory(string categoryName)
        {
            var category = _categoryService.GetCategoryByName(categoryName);
            if (category == null)
            {
                return NotFound(); 
            }

            int categoryId = category.Id;
            var posts = _postService.GetPostsByCategory(categoryId);

            if (posts == null || !posts.Any())
            {
                return Ok(new List<PostResponse>());
            }

            return Ok(posts);
        }


        [HttpGet("tag/{tagId}")]
        public IActionResult GetPostsByTag(int tagId)
        {
            var posts = _postService.GetPostsByTag(tagId);
            return Ok(posts);
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetPostsByUser(int userId)
        {
            var posts = _postService.GetPostsByUser(userId);
            return Ok(posts);
        }

        [HttpGet("search")]
        public IActionResult SearchPosts([FromQuery] string query)
        {
            var posts = _postService.SearchPosts(query);
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public IActionResult GetPostById(int id)
        {
            try
            {
                var post = _postService.GetPostById(id);
                return Ok(post);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message }); 
            }
        }

        [HttpGet("slug/{slug}")]
        public IActionResult GetPostBySlug(string slug)
        {
            try
            {
                var post = _postService.GetPostBySlug(slug);
                return Ok(post);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message }); 
            }
        }

        [HttpGet("all")]
        public IActionResult GetAllPosts()
        {
            var posts = _postService.GetAllPosts();
            return Ok(posts);
        }
    
        [HttpPost("{postId}/comments")]
        public IActionResult AddCommentOnPost(int postId, [FromBody] AddCommentRequest request)
        {
            try
            {
                _postService.AddCommentOnPost(postId, request.Name, request.Email, request.Content);
                return Ok(new { Message = "Comment added successfully" });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Details = ex.Message });
            }
        }
    }
}
