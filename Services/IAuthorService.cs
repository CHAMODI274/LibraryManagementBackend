using LibraryManagementBackend.Models;

namespace LibraryManagementBackend.Services
{
    // Service interface for author-related business operations
    // Defines the contract for author management functionality
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllAuthorsAsync(); // Retrieve all authors from the system
        Task<Author?> GetAuthorByIdAsync(int id); // Retrieve a specific author by their unique identifier
        Task<Author> CreateAuthorAsync(Author author); // Create a new author in the system
        Task<Author> UpdateAuthorAsync(int id, Author author); // Update an existing author's information
        Task<bool> DeleteAuthorAsync(int id); // Delete an author from the system
        Task<bool> AuthorExistsAsync(int id); // Check if an author exists in the system

    }
}
