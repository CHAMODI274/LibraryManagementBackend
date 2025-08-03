using LibraryManagementBackend.Models;
using LibraryManagementBackend.Services;
using LibraryManagementBackend.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;


namespace LibraryManagementBackend.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<LoanService> _logger;

        public LoanService(
            ILoanRepository loanRepository,
            IBookRepository bookRepository,
            ILogger<LoanService> logger)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Loan>> GetAllLoansAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all loans");
                return await _loanRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all loans");
                throw;
            }
        }

        public async Task<Loan?> GetLoanByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching loan with ID: {LoanId}", id);
                return await _loanRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching loan with ID: {LoanId}", id);
                throw;
            }
        }

        public async Task<Loan> CreateLoanAsync(Loan loan)
        {
            try
            {
                _logger.LogInformation("Creating new loan for book ID: {BookId} and user: {UserId}", loan.BookId, loan.UserId);

                // Business logic validation
                await ValidateLoanDataAsync(loan);

                // Check if book is available for borrowing
                if (!await CanBorrowBookAsync(loan.BookId))
                {
                    throw new InvalidOperationException("Book is currently not available for borrowing");
                }

                loan.BorrowedDate = DateTime.UtcNow;
                loan.Isreturned = false;

                var createdLoan = await _loanRepository.AddAsync(loan);

                _logger.LogInformation("Loan created successfully with ID: {LoanId}", createdLoan.Id);
                return createdLoan;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating loan for book ID: {BookId}", loan.BookId);
                throw;
            }
        }

        public async Task<Loan> UpdateLoanAsync(int id, Loan loan)
        {
            try
            {
                _logger.LogInformation("Updating loan with ID: {LoanId}", id);

                var existingLoan = await _loanRepository.GetByIdAsync(id);
                if (existingLoan == null)
                {
                    throw new KeyNotFoundException($"Loan with ID {id} not found");
                }

                // Business logic validation
                await ValidateLoanDataAsync(loan);

                existingLoan.UserId = loan.UserId;
                existingLoan.BookId = loan.BookId;
                existingLoan.BorrowedDate = loan.BorrowedDate;
                existingLoan.ReturnDate = loan.ReturnDate;
                existingLoan.Isreturned = loan.Isreturned;

                await _loanRepository.UpdateAsync(existingLoan);

                _logger.LogInformation("Loan updated successfully with ID: {LoanId}", id);
                return existingLoan;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating loan with ID: {LoanId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteLoanAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting loan with ID: {LoanId}", id);

                var exists = await _loanRepository.ExistsAsync(id);
                if (!exists)
                {
                    return false;
                }

                await _loanRepository.DeleteAsync(id);

                _logger.LogInformation("Loan deleted successfully with ID: {LoanId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting loan with ID: {LoanId}", id);
                throw;
            }
        }

        public async Task<bool> LoanExistsAsync(int id)
        {
            return await _loanRepository.ExistsAsync(id);
        }


        //------------

        public async Task<IEnumerable<Loan>> GetLoansByUserAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Fetching loans for user: {UserId}", userId);
                return await _loanRepository.GetByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching loans for user: {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<Loan>> GetActiveLoansByUserAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Fetching active loans for user: {UserId}", userId);
                var loans = await _loanRepository.GetByUserIdAsync(userId);
                return loans.Where(l => !l.Isreturned);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching active loans for user: {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<Loan>> GetOverdueLoansAsync()
        {
            try
            {
                _logger.LogInformation("Fetching overdue loans");
                return await _loanRepository.GetOverdueLoansAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching overdue loans");
                throw;
            }
        }


        public async Task<Loan> ReturnBookAsync(int loanId)
        {
            try
            {
                _logger.LogInformation("Processing book return for loan ID: {LoanId}", loanId);

                var loan = await _loanRepository.GetByIdAsync(loanId);
                if (loan == null)
                {
                    throw new KeyNotFoundException($"Loan with ID {loanId} not found");
                }

                if (loan.Isreturned)
                {
                    throw new InvalidOperationException("Book has already been returned");
                }

                loan.ReturnDate = DateTime.UtcNow;
                loan.Isreturned = true;

                await _loanRepository.UpdateAsync(loan);

                _logger.LogInformation("Book returned successfully for loan ID: {LoanId}", loanId);
                return loan;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing book return for loan ID: {LoanId}", loanId);
                throw;
            }
        }

        public async Task<bool> CanBorrowBookAsync(int bookId)
        {
            try
            {
                // Check if book exists
                if (!await _bookRepository.ExistsAsync(bookId))
                {
                    return false;
                }

                // Check if book has any active loans
                return !await _loanRepository.HasActiveLoanForBookAsync(bookId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking if book can be borrowed: {BookId}", bookId);
                throw;
            }
        }
        

        private async Task ValidateLoanDataAsync(Loan loan)
        {
            // Validate book exists
            if (!await _bookRepository.ExistsAsync(loan.BookId))
            {
                throw new ArgumentException($"Book with ID {loan.BookId} does not exist");
            }

            // Validate user ID is not empty
            if (string.IsNullOrWhiteSpace(loan.UserId))
            {
                throw new ArgumentException("User ID cannot be empty");
            }

            // Validate borrowed date is not in the future
            if (loan.BorrowedDate > DateTime.UtcNow)
            {
                throw new ArgumentException("Borrowed date cannot be in the future");
            }

            // If return date is provided, validate it's after borrowed date
            if (loan.ReturnDate.HasValue && loan.ReturnDate < loan.BorrowedDate)
            {
                throw new ArgumentException("Return date cannot be before borrowed date");
            }
        }  
    }

}