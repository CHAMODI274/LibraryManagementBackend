using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementBackend.Models;
using LibraryManagementBackend.Services;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(IAuthorService authorService, ILogger<AuthorsController> logger)
        {
            _authorService = authorService;
            _logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            try
            {
                _logger.LogInformation("Fetching all authors.");
                var authors = await _authorService.GetAllAuthorsAsync();
                if (authors == null || !authors.Any())
                {
                    _logger.LogWarning("No authors found.");
                    return NotFound("No authors found.");
                }
                return Ok(authors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching authors.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching author with ID {id}");
                var author = await _authorService.GetAuthorByIdAsync(id);

                if (author == null)
                {
                    _logger.LogWarning($"Author with ID {id} not found.");
                    return NotFound($"Author with ID {id} not found.");
                }

                return Ok(author);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the author.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            try
            {
                if (id != author.Id)
                {
                    _logger.LogWarning("Author ID mismatch.");
                    return BadRequest("Author ID mismatch.");
                }

                await _authorService.UpdateAuthorAsync(id, author);
                _logger.LogInformation($"Author with ID {id} updated.");
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _authorService.GetAuthorByIdAsync(id) == null)
                {
                    _logger.LogWarning($"Author with ID {id} not found for update.");
                    return NotFound($"Author with ID {id} not found.");
                }
                else
                {
                    _logger.LogError("Error updating author.");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the author.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/Students
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            try
            {
                if (author == null)
                {
                    _logger.LogWarning("Received empty author object.");
                    return BadRequest("Author data cannot be null.");
                }

                await _authorService.AddAuthorAsync(author);
                _logger.LogInformation($"Author with ID {author.Id} created.");
                return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the author.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                var author = await _authorService.GetAuthorByIdAsync(id);
                if (author == null)
                {
                    _logger.LogWarning($"Author with ID {id} not found.");
                    return NotFound($"Author with ID {id} not found.");
                }

                await _authorService.DeleteAuthorAsync(id);
                _logger.LogInformation($"Author with ID {id} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the author.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
