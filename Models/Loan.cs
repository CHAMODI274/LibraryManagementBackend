using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;


namespace LibraryManagementBackend.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }


        public int BookId { get; set; }
        public Book? Book { get; set; }


        public DateTime BorrowedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool Isreturned { get; set; }

    }
}
