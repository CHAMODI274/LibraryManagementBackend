using LibraryManagementBackend.Models;
using LibraryManagementBackend.Services;
using LibraryManagementBackend.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementBackend.Services
{

    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<AuthorService> _logger;


        public AuthorService(IAuthorRepository authorRepository, ILogger<AuthorService> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }


        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            try
            {
                _logger.LogInformation("fetching all authors");
                return await _authorRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all authors");
                throw;
            }
        }


        public async Task<Author?> GetAuthorByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching author with ID: {AuthorId}", id);
                return await _authorRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching author with ID: {AuthorId}", id);
                throw;
            }
        }


        public async Task<Author> CreateAuthorAsync(Author author)
        {
            try
            {
                _logger.LogInformation("Creating new author: {AuthorName}", author.Name);

                //Business logic validation
                var existingAuthor = await _authorRepository.GetByNameAsync(author.Name);
                if (existingAuthor != null)
                {
                    throw new InvalidOperationException($"Author with name '{author.Name}' already exists");
                }

                var createdAuthor = await _authorRepository.AddAsync(author);

                _logger.LogInformation("Author created successfully with ID: {AuthorId}", createdAuthor.Id);
                return createdAuthor;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating author: {AuthorName}", author.Name);
                throw;
            }
        }


        public async Task<Author> UpdateAuthorAsync(int id, Author author)
        {
            try
            {
                _logger.LogInformation("Updating author with ID: {AuthorId}", id);

                var existingAuthor = await _authorRepository.GetByIdAsync(id);
                if (existingAuthor == null)
                {
                    throw new KeyNotFoundException($"Author with ID {id} not found");
                }

                // Check for duplicate name (excluding current anuthor)
                var duplicateAuthor = await _authorRepository.GetByNameAsync(author.Name);
                if (duplicateAuthor != null && duplicateAuthor.Id != id)
                {
                    throw new InvalidOperationException($"Another author with name '{author.Name}' already exists");
                }

                existingAuthor.Name = author.Name;
                existingAuthor.Bio = author.Bio;

                await _authorRepository.UpdateAsync(existingAuthor);

                _logger.LogInformation("Author updated successfully with ID: {AuthorId}", id);
                return existingAuthor;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating author with ID: {AuthorId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting author with ID: {AuthorId}", id);

                var exists = await _authorRepository.ExistsAsync(id);
                if (!exists)
                {
                    return false;
                }

                await _authorRepository.DeleteAsync(id);

                _logger.LogInformation("Author deleted successfully with ID: {AuthorId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting author with ID: {AuthorId}", id);
                throw;
            }
        }
        
        public async Task<bool> AuthorExistsAsync(int id)
        {
            return await _authorRepository.ExistsAsync(id);
        }

    }
}



