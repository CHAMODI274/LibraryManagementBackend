using LibraryManagementBackend.Models;

namespace LibraryManagementBackend.Services
{
    public interface IPublisherService
    {
        Task<IEnumerable<Publisher>> GetAllPublishersAsync();
        Task<Publisher?> GetPublisherByIdAsync(int id);
        Task<Publisher> CreatePublisherAsync(Publisher publisher);
        Task<Publisher> UpdatePublisherAsync(int id, Publisher publisher);
        Task<bool> DeletePublisherAsync(int id);
        Task<bool> PublisherExistsAsync(int id);
    }
}
