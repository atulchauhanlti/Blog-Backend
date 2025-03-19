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

    public Category GetCategoryByName(string name)
    {
        return _categoryRepository.GetCategoryByName(name);
    }

    public void AddCategory(Category category)
    {
        _categoryRepository.AddCategory(category);
    }

    public void UpdateCategory(Category category)
    {
        _categoryRepository.UpdateCategory(category);
    }

    public void DeleteCategory(int id)
    {
        _categoryRepository.DeleteCategory(id);
    }
}
