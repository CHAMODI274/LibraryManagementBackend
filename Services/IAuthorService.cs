using LibraryManagementBackend.Models;

namespace LibraryManagementBackend.Services
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllAuthorsAsync();
        Task<Author?> GetAuthorByIdAsync(int id);
        Task AddAuthorAsync(Author author);
        Task UpdateAuthorAsync(int id, Author author);
        Task DeleteAuthorAsync(int id);
    }
}
