using LibraryManagementBackend.Models;
using LibraryManagementBackend.Services;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;


    public class PublisherService : IPublisherService
    {

        private readonly LibraryContext _context;


    public PublisherService(LibraryContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Publisher>> GetAllPublishersAsync()
    {
        return await _context.Publishers.ToListAsync();
    }


    public async Task<Publisher> GetPublisherByIdAsync(int id)
    {
        return await _context.Publishers.FindAsync(id);
    }


    public async Task AddPublisherAsync(Publisher publisher)
    {
        _context.Publishers.Add(publisher);
        await _context.SaveChangesAsync();
    }


    public async Task UpdatePublisherAsync(int id, Publisher publisher)
    {
            _context.Entry(publisher).State = EntityState.Modified;
            await _context.SaveChangesAsync();
    }

        public async Task  DeletePublisherAsync(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher != null)
            {
                _context.Publishers.Remove(publisher);
                await _context.SaveChangesAsync();
            }
        }
        
    }



