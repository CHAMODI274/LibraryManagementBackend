using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementBackend.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string ISBN { get; set; } = string.Empty;
        
        public int PublishedYear { get; set; }


        public int PublisherId { get; set; }
        public Publisher? Publisher { get; set; }


        public int CategoryId { get; set; }
        public Category? Category { get; set; }


        [JsonIgnore]
        public List<BookAuthor>? BookAuthors { get; set; }
        [JsonIgnore]
        public List<Loan>? Loans { get; set; }
    }
}
