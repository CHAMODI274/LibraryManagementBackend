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
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;
        private readonly ILogger<LoansController> _logger;

        public LoansController(ILoanService loanService, ILogger<LoansController> logger)
        {
            _loanService = loanService;
            _logger = logger;
        }

        // GET: api/Loans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoans()
        {
            try
            {
                _logger.LogInformation("Fetching all loans.");
                var loans = await _loanService.GetAllLoansAsync();
                if (loans == null || !loans.Any())
                {
                    _logger.LogWarning("No books found.");
                    return NotFound("No books found.");
                }
                return Ok(loans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching loans.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetLoan(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching loan with ID {id}");
                var loan = await _loanService.GetLoanByIdAsync(id);

                if (loan == null)
                {
                    _logger.LogWarning($"loan with ID {id} not found.");
                    return NotFound($"Loan with ID {id} not found.");
                }

                return Ok(loan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the loan.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/Loans/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoan(int id, Loan loan)
        {
            try
            {
                if (id != loan.Id)
                {
                    _logger.LogWarning("Loan ID mismatch.");
                    return BadRequest("Loan ID mismatch.");
                }

                await _loanService.UpdateLoanAsync(id, loan);
                _logger.LogInformation($"Loan with ID {id} updated.");
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _loanService.GetLoanByIdAsync(id) == null)
                {
                    _logger.LogWarning($"Loan with ID {id} not found for update.");
                    return NotFound($"Loan with ID {id} not found.");
                }
                else
                {
                    _logger.LogError("Error updating Loan.");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the loan.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/Loans
        [HttpPost]
        public async Task<ActionResult<Loan>> PostLoan(Loan loan)
        {
            try
            {
                if (loan == null)
                {
                    _logger.LogWarning("Received empty loan object.");
                    return BadRequest("loan data cannot be null.");
                }

                await _loanService.CreateLoanAsync(loan);
                _logger.LogInformation($"Loan with ID {loan.Id} created.");
                return CreatedAtAction("GetLoan", new { id = loan.Id }, loan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the loan.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/Loans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            try
            {
                var loan = await _loanService.GetLoanByIdAsync(id);
                if (loan == null)
                {
                    _logger.LogWarning($"Loan with ID {id} not found.");
                    return NotFound($"Loan with ID {id} not found.");
                }

                await _loanService.DeleteLoanAsync(id);
                _logger.LogInformation($"Loan with ID {id} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the Loan.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}

