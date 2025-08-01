using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementBackend.Models
{
    public class Loan
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public IdentityUser? User { get; set; }

        [Required]
        public int BookId { get; set; }
        
        public Book? Book { get; set; }


        public DateTime BorrowedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool Isreturned { get; set; }

    }
}
