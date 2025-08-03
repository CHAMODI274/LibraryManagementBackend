using LibraryManagementBackend.Models;

namespace LibraryManagementBackend.Repository
{
    public interface IPublisherRepository
    {
        Task<IEnumerable<Publisher>> GetAllAsync();
        Task<Publisher?> GetByIdAsync(int id);
        Task<Publisher> AddAsync(Publisher publisher);
        Task UpdateAsync(Publisher publisher);
        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
        Task<Publisher?> GetByNameAsync(string name);
    }
}