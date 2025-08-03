using LibraryManagementBackend.Models;
using LibraryManagementBackend.Repository;
using Microsoft.EntityFrameworkCore;


namespace LibraryManagementBackend.Repository
{
    public class LoanRepository : ILoanRepository
    {
        private readonly LibraryContext _context;

        public LoanRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Loan>> GetAllAsync()
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.User)
                .ToListAsync();
        }

        public async Task<Loan?> GetByIdAsync(int id)
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Loan> AddAsync(Loan loan)
        {
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
            return loan;
        }

        public async Task UpdateAsync(Loan loan)
        {
            _context.Entry(loan).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan != null)
            {
                _context.Loans.Remove(loan);
                await _context.SaveChangesAsync();
            }
        }


        //-----------

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Loans.AnyAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Loan>> GetByUserIdAsync(string userId)
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Where(l => l.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetByBookIdAsync(int bookId)
        {
            return await _context.Loans
                .Include(l => l.User)
                .Where(l => l.BookId == bookId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetActiveLoansAsync()
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.User)
                .Where(l => !l.Isreturned)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetOverdueLoansAsync()
        {
            var currentDate = DateTime.UtcNow;
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.User)
                .Where(l => !l.Isreturned && l.BorrowedDate.AddDays(14) < currentDate)
                .ToListAsync();
        }

        public async Task<bool> HasActiveLoanForBookAsync(int bookId)
        {
            return await _context.Loans
                .AnyAsync(l => l.BookId == bookId && !l.Isreturned);
        }
    }
}


