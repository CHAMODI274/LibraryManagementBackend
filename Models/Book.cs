using System.Collections.Generic;
using Microsoft.Extensions.Diagnostics.HealthChecks;


namespace LibraryManagementBackend.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ISBN { get; set; }
        public int PublishedYear { get; set; }


        public int PublisherId { get; set; }
        public Publisher? Publisher { get; set; }


        public int CategoryId { get; set; }
        public Category? Category { get; set; }


        public List<BookAuthor>? BookAuthors { get; set; }
        public List<Loan>? Loans { get; set; }
    }
}
