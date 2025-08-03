using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryManagementBackend.Models;
using LibraryManagementBackend.Repository;

namespace LibraryManagementBackend.Repository
{
    public class BookRepository : IBookRepository
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
                    .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
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

        public async Task<Book> AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
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

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Books.AnyAsync(b => b.Id == id);
        }

        public async Task<Book?> GetByISBNAsync(string isbn)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        public async Task<IEnumerable<Book>> GetByAuthorIdAsync(int authorId)
        {
            return await _context.Books
                .Include(b => b.BookAuthors)
                .Where(b => b.BookAuthors.Any(ba => ba.AuthorId == authorId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetByCategoryIdAsync(int categoryId)
        {
            return await _context.Books
                .Where(b => b.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetByPublisherIdAsync(int publisherId)
        {
            return await _context.Books
                .Where(b => b.PublisherId == publisherId)
                .ToListAsync();
        }
    }
}
