using LibraryManagementBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryManagementBackend.Services
{
    public interface ILoanService
    {
        Task<IEnumerable<Loan>> GetAllLoansAsync();
        Task<Loan?> GetLoanByIdAsync(int id);
        Task AddLoanAsync(Loan loan);
        Task UpdateLoanAsync(int id, Loan loan);
        Task DeleteLoanAsync(int id);
    }
}