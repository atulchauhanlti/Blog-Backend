using System;
using System.Linq;
using backend.Models.PostDtos;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly PostService _postService;

        public PostsController(PostService postService)
        {
            _postService = postService;
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
                // Save the image file to the server (e.g., in an "uploads" directory)
                var filePath = Path.Combine("uploads", imageFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }
                imageUrl = filePath; // Store the file path or convert it to a public URL if hosted
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
                // Save the new image file
                var filePath = Path.Combine("uploads", imageFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }
                imageUrl = filePath; // Store the file path or convert it to a public URL
            }

            _postService.UpdatePost(id, request, imageUrl); // Pass id, request, and image URL to service layer
            return NoContent();
        }


        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeletePost(int id)
        {
            _postService.DeletePost(id);
            return NoContent();
        }


        [HttpGet("category/{categoryId}")]
        public IActionResult GetPostsByCategory(int categoryId)
        {
            var posts = _postService.GetPostsByCategory(categoryId);
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

        // [HttpGet("{slug}")]
        // public IActionResult GetPostBySlug(string slug)
        // {
        //     var post = _postService.GetPostBySlug(slug);
        //     return Ok(post);
        // }

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
                return NotFound(new { Message = ex.Message }); // Return a 404 with an error message
            }
        }

        [HttpGet("all")]
        public IActionResult GetAllPosts()
        {
            var posts = _postService.GetAllPosts();
            return Ok(posts);
        }
    }
}
