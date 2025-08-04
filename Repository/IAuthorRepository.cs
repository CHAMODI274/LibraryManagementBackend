using LibraryManagementBackend.Models;

namespace LibraryManagementBackend.Repository
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author?> GetByIdAsync(int id);
        Task<Author> AddAsync(Author author);
        Task UpdateAsync(Author author);
        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id); 
        Task<Author?> GetByNameAsync(string name);

    }
}