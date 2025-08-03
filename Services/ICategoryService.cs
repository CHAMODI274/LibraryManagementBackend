using LibraryManagementBackend.Models;

namespace LibraryManagementBackend.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(int id, Category category);
        Task DeleteCategoryAsync(int id);
    }
}
