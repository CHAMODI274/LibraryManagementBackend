using LibraryManagementBackend.Models;
using LibraryManagementBackend.Services;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;


    public class CategoryService : ICategoryService
    {

        private readonly LibraryContext _context;


    public CategoryService(LibraryContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }


    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }


    public async Task AddCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }


    public async Task UpdateCategoryAsync(int id, Category category)
    {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
    }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
        
    }



