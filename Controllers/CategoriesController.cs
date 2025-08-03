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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }


        // GET: api/Categorys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            try
            {
                _logger.LogInformation("Fetching all categories.");
                var Categories = await _categoryService.GetAllCategoriesAsync();
                if (Categories == null || !Categories.Any())
                {
                    _logger.LogWarning("No categories found.");
                    return NotFound("No categories found.");
                }
                return Ok(Categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching categories.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching category with ID {id}");
                var category = await _categoryService.GetCategoryByIdAsync(id);

                if (category == null)
                {
                    _logger.LogWarning($"Category with ID {id} not found.");
                    return NotFound($"Category with ID {id} not found.");
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the category.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            try
            {
                if (id != category.Id)
                {
                    _logger.LogWarning("Category ID mismatch.");
                    return BadRequest("Category ID mismatch.");
                }

                await _categoryService.UpdateCategoryAsync(id, category);
                _logger.LogInformation($"Category with ID {id} updated.");
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _categoryService.GetCategoryByIdAsync(id) == null)
                {
                    _logger.LogWarning($"Category with ID {id} not found for update.");
                    return NotFound($"Category with ID {id} not found.");
                }
                else
                {
                    _logger.LogError("Error updating category.");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the category.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<Category>> Postcategory(Category category)
        {
            try
            {
                if (category == null)
                {
                    _logger.LogWarning("Received empty category object.");
                    return BadRequest("category data cannot be null.");
                }

                await _categoryService.AddCategoryAsync(category);
                _logger.LogInformation($"Category with ID {category.Id} created.");
                return CreatedAtAction("GetCategory", new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the category.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning($"Category with ID {id} not found.");
                    return NotFound($"Category with ID {id} not found.");
                }

                await _categoryService.DeleteCategoryAsync(id);
                _logger.LogInformation($"Category with ID {id} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the category.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
