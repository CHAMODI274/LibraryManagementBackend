using System.ComponentModel.DataAnnotations;

namespace LibraryManagementBackend.Models
{
    public class LoanDTO
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [Required]
        public int BookId { get; set; }
        
        public DateTime BorrowedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool Isreturned { get; set; }

    }
}
