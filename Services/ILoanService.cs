using LibraryManagementBackend.Models;

namespace LibraryManagementBackend.Services
{
    public interface ILoanService
    {
        Task<IEnumerable<Loan>> GetAllLoansAsync();
        Task<Loan?> GetLoanByIdAsync(int id);
        Task<Loan> CreateLoanAsync(Loan loan);
        Task<Loan> UpdateLoanAsync(int id, Loan loan);
        Task<bool> DeleteLoanAsync(int id);

        Task<bool> LoanExistsAsync(int id);
        Task<IEnumerable<Loan>> GetLoansByUserAsync(string userId);
        Task<IEnumerable<Loan>> GetActiveLoansByUserAsync(string userId);
        Task<IEnumerable<Loan>> GetOverdueLoansAsync();
        Task<Loan> ReturnBookAsync(int loanId);
        Task<bool> CanBorrowBookAsync(int bookId);
    }
}