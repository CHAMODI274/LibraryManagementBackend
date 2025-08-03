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
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherService _publisherService;
        private readonly ILogger<PublishersController> _logger;

        public PublishersController(IPublisherService publisherService, ILogger<PublishersController> logger)
        {
            _publisherService = publisherService;
            _logger = logger;
        }

        // GET: api/Publishers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publisher>>> GetPublishers()
        {
            try
            {
                _logger.LogInformation("Fetching all publishers.");
                var publishers = await _publisherService.GetAllPublishersAsync();
                if (publishers == null || !publishers.Any())
                {
                    _logger.LogWarning("No publishers found.");
                    return NotFound("No publishers found.");
                }
                return Ok(publishers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching publisherss.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Publishers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Publisher>> GetPublisher(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching publisher with ID {id}");
                var publisher = await _publisherService.GetPublisherByIdAsync(id);

                if (publisher == null)
                {
                    _logger.LogWarning($"Publisher with ID {id} not found.");
                    return NotFound($"Publisher with ID {id} not found.");
                }

                return Ok(publisher);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the publisher.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/Publishers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublisher(int id, Publisher publisher)
        {
            try
            {
                if (id != publisher.Id)
                {
                    _logger.LogWarning("Publisher ID mismatch.");
                    return BadRequest("Publisher ID mismatch.");
                }

                await _publisherService.UpdatePublisherAsync(id, publisher);
                _logger.LogInformation($"Publisher with ID {id} updated.");
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _publisherService.GetPublisherByIdAsync(id) == null)
                {
                    _logger.LogWarning($"Publisher with ID {id} not found for update.");
                    return NotFound($"Publisher with ID {id} not found.");
                }
                else
                {
                    _logger.LogError("Error updating publisher.");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the publisher.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/Publishers
        [HttpPost]
        public async Task<ActionResult<Publisher>> PostPublisher(Publisher publisher)
        {
            try
            {
                if (publisher == null)
                {
                    _logger.LogWarning("Received empty publisher object.");
                    return BadRequest("Publisher data cannot be null.");
                }

                await _publisherService.AddPublisherAsync(publisher);
                _logger.LogInformation($"Publisher with ID {publisher.Id} created.");
                return CreatedAtAction("GetPublisher", new { id = publisher.Id }, publisher);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the author.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/Publishers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            try
            {
                var publisher = await _publisherService.GetPublisherByIdAsync(id);
                if (publisher == null)
                {
                    _logger.LogWarning($"Publisher with ID {id} not found.");
                    return NotFound($"Publisher with ID {id} not found.");
                }

                await _publisherService.DeletePublisherAsync(id);
                _logger.LogInformation($"Publisher with ID {id} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the publisher.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
