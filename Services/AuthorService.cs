using LibraryManagementBackend.Models;
using LibraryManagementBackend.Services;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;


    public class AuthorService : IAuthorService
    {

        private readonly LibraryContext _context;


    public AuthorService(LibraryContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
    {
        return await _context.Authors.ToListAsync();
    }


    public async Task<Author> GetAuthorByIdAsync(int id)
    {
        return await _context.Authors.FindAsync(id);
    }


    public async Task AddAuthorAsync(Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
    }


    public async Task UpdateAuthorAsync(int id, Author author)
    {
            _context.Entry(author).State = EntityState.Modified;
            await _context.SaveChangesAsync();
    }

        public async Task DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }
        }
        
    }



