using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryManagementBackend.Models;


namespace LibraryManagementBackend.Repositories
{
    public class BookRepository 
    {
        private readonly LibraryContext _context;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }

        // Get all books — optionally include Publisher and Category
        public async Task<IEnumerable<Book>> GetAllAsync(bool includeRelated = false)
        {
            if (includeRelated)
            {
                return await _context.Books
                    .Include(b => b.Publisher)
                    .Include(b => b.Category)
                    .ToListAsync();
            }

            return await _context.Books.ToListAsync();
        }


        // Get book by ID — optionally include authors, publisher, and category
        public async Task<Book?> GetByIdAsync(int id, bool includeRelated = false)
        {
            if (includeRelated)
            {
                return await _context.Books
                    .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
                    .Include(b => b.Publisher)
                    .Include(b => b.Category)
                    .FirstOrDefaultAsync(b => b.Id == id);
            }

            return await _context.Books.FindAsync(id);
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
    }
}
