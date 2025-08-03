using LibraryManagementBackend.Models;

namespace LibraryManagementBackend.Services
{
    public interface IPublisherService
    {
        Task<IEnumerable<Publisher>> GetAllPublishersAsync();
        Task<Publisher?> GetPublisherByIdAsync(int id);
        Task AddPublisherAsync(Publisher publisher);
        Task UpdatePublisherAsync(int id, Publisher publisher);
        Task DeletePublisherAsync(int id);
    }
}
