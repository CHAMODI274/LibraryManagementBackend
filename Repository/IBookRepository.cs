using LibraryManagementBackend.Models;

namespace LibraryManagementBackend.Repository
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync(bool includeRelated = false);
        Task<Book?> GetByIdAsync(int id, bool includeRelated = false);
        Task<Book> AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
        Task<Book?> GetByISBNAsync(string isbn);
        Task<IEnumerable<Book>> GetByAuthorIdAsync(int AuthorId);
        Task<IEnumerable<Book>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Book>> GetByPublisherIdAsync(int PublisherId);
    }
}

