using LibraryManagementBackend.Models;
using LibraryManagementBackend.Repository;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementBackend.Repository
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly LibraryContext _context;

        public PublisherRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Publisher>> GetAllAsync()
        {
            return await _context.Publishers.ToListAsync();
        }

        public async Task<Publisher?> GetByIdAsync(int id)
        {
            return await _context.Publishers.FindAsync(id);
        }

        public async Task<Publisher> AddAsync(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();
            return publisher;
        }

        public async Task UpdateAsync(Publisher publisher)
        {
            _context.Entry(publisher).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher != null)
            {
                _context.Publishers.Remove(publisher);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Publishers.AnyAsync(p => p.Id == id);
        }

        public async Task<Publisher?> GetByNameAsync(string name)
        {
            return await _context.Publishers.FirstOrDefaultAsync(p => p.Name == name);
        }
    }
}


