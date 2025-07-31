using LibraryManagementBackend.Models;
using LibraryManagementBackend.Services;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;


    public class BookService : IBookService
    {

        private readonly LibraryContext _context;


    public BookService(LibraryContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        return await _context.Books.ToListAsync();
    }


    public async Task<Book> GetBookByIdAsync(int id)
    {
        return await _context.Books.FindAsync(id);
    }


    public async Task AddBookAsync(Book book)
    {
        //ISBM must be Unique
        var existingBook = await _context.Books.FirstOrDefaultAsync(b => b.ISBN == book.ISBN);
        if (existingBook != null)
            throw new InvalidOperationException("A book with this ISBN already exists.");

        _context.Books.Add(book);
        await _context.SaveChangesAsync();
    }


    public async Task UpdateBookAsync(int id, Book book)
    {
            if (id != book.Id)
                throw new ArgumentException("Book ID mismatch");

        //prevent ISBN duplication on update
                var existing = await _context.Books
                .FirstOrDefaultAsync(b => b.ISBN == book.ISBN && b.Id != id);
                if (existing != null)
                     throw new InvalidOperationException("Another book with this ISBN already exists.");


            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
    }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
        
    }



