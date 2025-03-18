using System.Collections.Generic;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/categories
        [HttpGet]
        public IEnumerable<Category> GetCategories()
        {
            return _categoryService.GetAllCategories();
        }

        // GET: api/categories/{id}
        [HttpGet("{id}")]
        public ActionResult<Category> GetCategoryById(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return category;
        }

        // POST: api/categories
        [HttpPost]
        public IActionResult AddCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _categoryService.AddCategory(category); // Call the service layer
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        // PUT: api/categories/{id}
       [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] Category category)
        {
            var existingCategory = _categoryService.GetCategoryById(id); // This is already tracked by DbContext
            if (existingCategory == null)
            {
                return NotFound();
            }

            // Update the tracked instance with new values
            existingCategory.Name = category.Name;

            _categoryService.UpdateCategory(existingCategory);
            return NoContent();
        }

        // DELETE: api/categories/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            _categoryService.DeleteCategory(id);
            return NoContent();
        }
    }
}
