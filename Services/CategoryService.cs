using System;
using System.Collections.Generic;
using backend.Interface;
using backend.Models;

namespace backend.Services;

public class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public IEnumerable<Category> GetAllCategories()
    {
        return _categoryRepository.GetAllCategories();
    }

    public Category GetCategoryById(int id)
    {
        return _categoryRepository.GetCategoryById(id);
    }

    public void AddCategory(Category category)
    {
        // Add any additional validation or business logic here
        _categoryRepository.AddCategory(category);
    }

    public void UpdateCategory(Category category)
    {
        // Additional logic like checking if the category exists
        _categoryRepository.UpdateCategory(category);
    }

    public void DeleteCategory(int id)
    {
        // Additional logic before deletion
        _categoryRepository.DeleteCategory(id);
    }
}
