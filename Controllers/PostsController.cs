using System;
using System.Linq;
using backend.Models.PostDtos;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost]
        public IActionResult AddPost([FromBody] PostCreateRequest request)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return Unauthorized("User ID is missing in the token.");
            }

            var userId = int.Parse(userIdClaim.Value); 
            _postService.AddPost(request, userId);
            return Ok();
        }

        [Authorize]
        [HttpPut("{slug}")]
        public IActionResult UpdatePost(string slug, [FromBody] PostUpdateRequest request)
        {
            _postService.UpdatePost(request);
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

        [HttpGet("{slug}")]
        public IActionResult GetPostBySlug(string slug)
        {
            var post = _postService.GetPostBySlug(slug);
            return Ok(post);
        }
    }
}
