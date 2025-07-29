// Join table for many-to many relationship between Book & Author


using System.Collections.Generic;


namespace LibraryManagementBackend.Models
{
    public class BookAuthor
    {
        public int BookId { get; set; }
        public Book? Book { get; set; }


        public int AuthorId { get; set; }
        public Author? Author { get; set; }
   
    }
}
