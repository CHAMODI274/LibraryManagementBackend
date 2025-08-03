using LibraryManagementBackend.Models;
using LibraryManagementBackend.Services;
using LibraryManagementBackend.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementBackend.Services
{
    public class BookService : IBookService
    {

        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPublisherRepository _publisherRepository;
        private readonly ILogger<BookService> _logger;


        public BookService(
            IBookRepository bookRepository,
            IAuthorRepository authorRepository,
            ICategoryRepository categoryRepository,
            IPublisherRepository publisherRepository,
            ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _publisherRepository = publisherRepository;
            _logger = logger;
        }


        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all books");
                return await _bookRepository.GetAllAsync(includeRelated: true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all books");
                throw;
            }
        }


        public async Task<Book?> GetBookByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("fetching book with ID: {BookId}", id);
                return await _bookRepository.GetByIdAsync(id, includeRelated: true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching book with ID: {BookId}", id);
                throw;
            }
        }


        public async Task<Book> CreateBookAsync(Book book)
        {
            try
            {
                _logger.LogInformation("Creating new book: {BookTitle}", book.Title);

                // Business logic validation
                await ValidateBookDataAsync(book);

                // Check for duplicate ISBN
                var existingBook = await _bookRepository.GetByISBNAsync(book.ISBN);
                if (existingBook != null)
                {
                    throw new InvalidOperationException($"A book with ISBN '{book.ISBN}' already exists");
                }

                var CreateBook = await _bookRepository.AddAsync(book);

                _logger.LogInformation("Book created sucssesfully with ID: {BookId}", CreateBook.Id);
                return CreateBook;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating book: {BookTitle}", book.Title);
                throw;
            }

        }



        public async Task<Book> UpdateBookAsync(int id, Book book)
        {
            try
            {
                _logger.LogInformation("Updating book with ID: {BookId}", id);

                var existingBook = await _bookRepository.GetByIdAsync(id);
                if (existingBook == null)
                {
                    throw new KeyNotFoundException($"Book with ID {id} not found");
                }

                // Business logic validation
                await ValidateBookDataAsync(book);

                // Check for duplicate ISBN (excluding current book)
                var duplicateBook = await _bookRepository.GetByISBNAsync(book.ISBN);
                if (duplicateBook != null && duplicateBook.Id != id)
                {
                    throw new InvalidOperationException($"Another book with ISBN '{book.ISBN}' already exists");
                }

                existingBook.Title = book.Title;
                existingBook.ISBN = book.ISBN;
                existingBook.PublishedYear = book.PublishedYear;
                existingBook.PublisherId = book.PublisherId;
                existingBook.CategoryId = book.CategoryId;

                await _bookRepository.UpdateAsync(existingBook);

                _logger.LogInformation("Book updated successfully with ID: {BookId}", id);
                return existingBook;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating book with ID: {BookId}", id);
                throw;
            }
        }



        public async Task<bool> DeleteBookAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting book with ID: {BookId}", id);

                var exists = await _bookRepository.ExistsAsync(id);
                if (!exists)
                {
                    return false;
                }

                await _bookRepository.DeleteAsync(id);

                _logger.LogInformation("Book deleted successfully with ID: {BookId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting book with ID: {BookId}", id);
                throw;
            }
        }


        //-----------------------------


        public async Task<bool> BookExistsAsync(int id)
        {
            return await _bookRepository.ExistsAsync(id);
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId)
        {
            try
            {
                _logger.LogInformation("Fetching books by author ID: {AuthorId}", authorId);
                return await _bookRepository.GetByAuthorIdAsync(authorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching books by author ID: {AuthorId}", authorId);
                throw;
            }
        }


        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId)
        {
            try
            {
                _logger.LogInformation("Fetching books by category ID: {CategoryId}", categoryId);
                return await _bookRepository.GetByCategoryIdAsync(categoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching books by category ID: {CategoryId}", categoryId);
                throw;
            }
        }


        public async Task<IEnumerable<Book>> GetBooksByPublisherAsync(int publisherId)
        {
            try
            {
                _logger.LogInformation("Fetching books by publisher ID: {PublisherId}", publisherId);
                return await _bookRepository.GetByPublisherIdAsync(publisherId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching books by publisher ID: {PublisherId}", publisherId);
                throw;
            }
        }


        // -----------------------------------------------------------------------------------------------------

        private async Task ValidateBookDataAsync(Book book)
        {
            // Validate publisher exists
            if (!await _publisherRepository.ExistsAsync(book.PublisherId))
            {
                throw new ArgumentException($"Publisher with ID {book.PublisherId} dose not exist");
            }

            // Validate category exists
            if (!await _categoryRepository.ExistsAsync(book.CategoryId))
            {
                throw new ArgumentException($"Category with ID {book.CategoryId} does not exist");
            }

            // Validate published year
            if (book.PublishedYear < 1000)
            {
                throw new ArgumentException("Published year must be a valid year");
            }

            if (book.PublishedYear > DateTime.Now.Year)
            {
                throw new ArgumentException("Published year cannot be in the future");
            }
        }
    }
}



