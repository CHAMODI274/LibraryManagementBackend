using LibraryManagementBackend.Models;
using LibraryManagementBackend.Services;
using LibraryManagementBackend.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementBackend.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }


        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all categories");
                return await _categoryRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all categories");
                throw;
            }
        }


        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching category with ID: {CategoryId}", id);
                return await _categoryRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching category with ID: {CategoryId}", id);
                throw;
            }
        }


        public async Task<Category> CreateCategoryAsync(Category category)
        {
            try
            {
                _logger.LogInformation("Creating new category: {CategoryName}", category.Name);

                // Business logic validation
                var existingCategory = await _categoryRepository.GetByNameAsync(category.Name);
                if (existingCategory != null)
                {
                    throw new InvalidOperationException($"Category with name '{category.Name}' already exists");
                }

                var createdCategory = await _categoryRepository.AddAsync(category);

                _logger.LogInformation("Category created successfully with ID: {CategoryId}", createdCategory.Id);
                return createdCategory;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating category: {CategoryName}", category.Name);
                throw;
            }
        }


        public async Task<Category> UpdateCategoryAsync(int id, Category category)
        {
            try
            {
                _logger.LogInformation("Updating category with ID: {CategoryId}", id);

                var existingCategory = await _categoryRepository.GetByIdAsync(id);
                if (existingCategory == null)
                {
                    throw new KeyNotFoundException($"Category with ID {id} not found");
                }

                // Check for duplicate name (excluding current category)
                var duplicateCategory = await _categoryRepository.GetByNameAsync(category.Name);
                if (duplicateCategory != null && duplicateCategory.Id != id)
                {
                    throw new InvalidOperationException($"Another category with name '{category.Name}' already exists");
                }

                existingCategory.Name = category.Name;

                await _categoryRepository.UpdateAsync(existingCategory);

                _logger.LogInformation("Category updated successfully with ID: {CategoryId}", id);
                return existingCategory;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating category with ID: {CategoryId}", id);
                throw;
            }
        }


        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting category with ID: {CategoryId}", id);

                var exists = await _categoryRepository.ExistsAsync(id);
                if (!exists)
                {
                    return false;
                }

                await _categoryRepository.DeleteAsync(id);

                _logger.LogInformation("Category deleted successfully with ID: {CategoryId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting category with ID: {CategoryId}", id);
                throw;
            }
        }
        
        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _categoryRepository.ExistsAsync(id);
        }

    }
}



