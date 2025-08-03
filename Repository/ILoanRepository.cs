using LibraryManagementBackend.Models;

namespace LibraryManagementBackend.Repository
{
    public interface ILoanRepository
    {
        Task<IEnumerable<Loan>> GetAllAsync();
        Task<Loan?> GetByIdAsync(int id);
        Task<Loan> AddAsync(Loan loan);
        Task UpdateAsync(Loan loan);
        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Loan>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Loan>> GetByBookIdAsync(int bookId);
        Task<IEnumerable<Loan>> GetActiveLoansAsync();
        Task<IEnumerable<Loan>> GetOverdueLoansAsync();
        Task<bool> HasActiveLoanForBookAsync(int bookId);
    }
}