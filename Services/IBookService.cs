using LibraryManagementBackend.Models;
//using System.Threading.Tasks;
//using System.Collections.Generic;

namespace LibraryManagementBackend.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(int id, Book book);
        Task DeleteBookAsync(int id);
    }
}
