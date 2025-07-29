using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace LibraryManagementBackend.Models
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }


        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<BookAuthor>()
                 .HasKey(ba => new { ba.BookId, ba.AuthorId });


            modelBuilder.Entity<BookAuthor>()
                 .HasOne(ba => ba.Book)
                 .WithMany(ba => ba.BookAuthors)
                 .HasForeignKey(ba => ba.AuthorId);  


            modelBuilder.Entity<BookAuthor>()
                 .HasOne(ba => ba.Author)
                 .WithMany(ba => ba.BookAuthors)
                 .HasForeignKey(ba => ba.AuthorId);  
        }
     }
}


