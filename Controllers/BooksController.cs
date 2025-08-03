using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementBackend.Models;
using LibraryManagementBackend.Services;
using Swashbuckle.AspNetCore.Annotations;


namespace LibraryManagementBackend.Controllers
{
    // Controller for mapping books in the library system
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookService bookService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        // GET: api/Books
        // Retrieves all books from the library system
                // returns- A list of all books
                    // response code="200">Returns the list of books
                    // response code="404">If no books are found
                    // response code="500">If there was an internal server error
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            try
            {
                _logger.LogInformation("Fetching all books.");
                var books = await _bookService.GetAllBooksAsync();

                if (books == null || !books.Any())
                {
                    _logger.LogWarning("No books found.");
                    return NotFound("No books found.");
                }
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching books.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Books/5
        // Retrieves a specific book by its ID
         // name="id" The unique identifier of the book
             // returns The book with the specified ID
               // response code="200">Returns the requested book
               // response code="400">If the book ID is invalid
               // response code="404">If the book is not found
               // response code="500">If there was an internal server error
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid book ID: {BookId}", id);
                    return BadRequest("Invalid book ID");
                }
                
                _logger.LogInformation("Fetching book with ID: {BookId}", id);
                var book = await _bookService.GetBookByIdAsync(id);

                if (book == null)
                {
                    _logger.LogWarning("Book with ID {BookId} not found.",id);
                    return NotFound($"Book with ID {id} not found.");
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the book with ID: {BookId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/Books/5
        // Updates an existing book
        // name="id" The unique identifier of the book to update
        // name="book" The updated book information
        // returns - The updated book
               // response code="200">Successfully updated the book
               // response code="400">If the request data is invalid
               // response code="404">If the book is not found
               // response code="500">If there was an internal server error
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid book ID: {BookId}", id);
                    return BadRequest("Invalid book ID");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for book update");
                    return BadRequest(ModelState); 
                }

                _logger.LogInformation("Updating book with ID: {BookId}", id);
                var updatedBook = await _bookService.UpdateBookAsync(id, book);
                
                return Ok(updatedBook);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Book with ID {BookId} not found for update", id);
                return NotFound(ex.Message);
            }

            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while updating book with ID: {BookId}", id);
                return BadRequest(ex.Message);
            }

            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument while updating book with ID: {BookId}", id);
                return BadRequest(ex.Message);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the book with ID: {BookId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }


        // POST: api/Books
        // Creates a new book
        // name="book" The book information to create
        // returns - The newly created book
        // response code="201">Successfully created the book
        // response code="400">If the request data is invalid
        // response code="500">If there was an internal server error
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook([FromBody] Book book)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for book creation");
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Creating new book: {BookTitle}", book.Title);
                var createdBook = await _bookService.CreateBookAsync(book);
                
                return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while creating book: {BookTitle}", book.Title);
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument while creating book: {BookTitle}", book.Title);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating book: {BookTitle}", book.Title);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/Students/5
        // Deletes a book
            // name="id" The unique identifier of the book to delete
             // returns - No content if successful
                 // response code="204">Successfully deleted the book
                 // response code="400">If the book ID is invalid
                 // response code="404">If the book is not found
                 // response code="500">If there was an internal server error
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid book ID: {BookId}", id);
                    return BadRequest("Invalid book ID");
                }

                _logger.LogInformation("Deleting book with ID: {BookId}", id);
                var deleted = await _bookService.DeleteBookAsync(id);
                
                if (!deleted)
                {
                    _logger.LogWarning("Book with ID {BookId} not found for deletion", id);
                    return NotFound($"Book with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting book with ID: {BookId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}

