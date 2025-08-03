using LibraryManagementBackend.Models;

namespace LibraryManagementBackend.Repository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
        Task<Category?> GetByNameAsync(string name);
    }
}