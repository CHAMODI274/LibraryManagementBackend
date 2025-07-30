using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementBackend.Models;
using LibraryManagementBackend.Services;


namespace LibraryManagementBackend.Controllers
{
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
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching book with ID {id}");
                var book = await _bookService.GetBookByIdAsync(id);

                if (book == null)
                {
                    _logger.LogWarning($"Book with ID {id} not found.");
                    return NotFound($"Book with ID {id} not found.");
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the book.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            try
            {
                if (id != book.Id)
                {
                    _logger.LogWarning("Book ID mismatch.");
                    return BadRequest("Book ID mismatch.");
                }

                await _bookService.UpdateBookAsync(id, book);
                _logger.LogInformation($"Book with ID {id} updated.");
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _bookService.GetBookByIdAsync(id) == null)
                {
                    _logger.LogWarning($"Book with ID {id} not found for update.");
                    return NotFound($"Book with ID {id} not found.");
                }
                else
                {
                    _logger.LogError("Error updating book.");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the book.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            try
            {
                if (book == null)
                {
                    _logger.LogWarning("Received empty book object.");
                    return BadRequest("Book data cannot be null.");
                }

                await _bookService.AddBookAsync(book);
                _logger.LogInformation($"Book with ID {book.Id} created.");
                return CreatedAtAction("GetBook", new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the book.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    _logger.LogWarning($"Book with ID {id} not found.");
                    return NotFound($"Book with ID {id} not found.");
                }

                await _bookService.DeleteBookAsync(id);
                _logger.LogInformation($"Book with ID {id} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the .");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}

