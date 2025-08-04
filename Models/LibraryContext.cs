using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;



namespace LibraryManagementBackend.Models
{
    public class LibraryContext : IdentityDbContext<IdentityUser>
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

          // Configure BookAuthor many-to-many relationship
               modelBuilder.Entity<BookAuthor>()
                    .HasKey(ba => new { ba.BookId, ba.AuthorId });

          // Book -> BookAuthor relationship
               modelBuilder.Entity<BookAuthor>()
                    .HasOne(ba => ba.Book)
                    .WithMany(ba => ba.BookAuthors)
                    .HasForeignKey(ba => ba.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);

          // Author -> BookAuthor relationship
               modelBuilder.Entity<BookAuthor>()
                    .HasOne(ba => ba.Author)
                    .WithMany(ba => ba.BookAuthors)
                    .HasForeignKey(ba => ba.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade); 

               // Book -> Publisher relationship (one-to-many)
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting publisher if books exist

            // Book -> Category relationship (one-to-many)
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting category if books exist

            // Loan -> Book relationship (one-to-many)
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting book if loans exist 
        }
     }
}


