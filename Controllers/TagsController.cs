using System.Collections.Generic;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly TagService _tagService;

        public TagsController(TagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public IEnumerable<Tag> GetTags()
        {
            return _tagService.GetAllTags();
        }

        [HttpGet("{id}")]
        public ActionResult<Tag> GetTagById(int id)
        {
            var tag = _tagService.GetTagById(id);
            if (tag == null)
            {
                return NotFound();
            }
            return tag;
        }

        [HttpPost]
        public IActionResult AddTag([FromBody] Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _tagService.AddTag(tag);
            return CreatedAtAction(nameof(GetTagById), new { id = tag.Id }, tag);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTag(int id, [FromBody] Tag tag)
        {
            if (id != tag.Id)
            {
                return BadRequest();
            }

            var existingTag = _tagService.GetTagById(id);
            if (existingTag == null)
            {
                return NotFound();
            }

            _tagService.UpdateTag(tag);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTag(int id)
        {
            var tag = _tagService.GetTagById(id);
            if (tag == null)
            {
                return NotFound();
            }

            _tagService.DeleteTag(id);
            return NoContent();
        }
    }
}
