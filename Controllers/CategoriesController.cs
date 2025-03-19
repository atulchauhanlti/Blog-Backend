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

        [HttpGet]
        public IEnumerable<Category> GetCategories()
        {
            return _categoryService.GetAllCategories();
        }

        [HttpGet("name/{name}")]
        public ActionResult<int> GetCategoryIdByName(string name)
        {
            var category = _categoryService.GetCategoryByName(name);
            if (category == null)
            {
                return NotFound();
            }
            return category.Id;
        }

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

        [HttpPost]
        public IActionResult AddCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _categoryService.AddCategory(category); 
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

       [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] Category category)
        {
            var existingCategory = _categoryService.GetCategoryById(id); 
            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.Name = category.Name;

            _categoryService.UpdateCategory(existingCategory);
            return NoContent();
        }

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
