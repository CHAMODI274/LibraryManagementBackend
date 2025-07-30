using System.ComponentModel.DataAnnotations;

namespace LibraryManagementBackend.Models
{
    public class BookDTO
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string ISBN { get; set; } = string.Empty;

        public int PublishedYear { get; set; }
        public int PublisherId { get; set; }
        public int CategoryId { get; set; }
        
        public List<int>? AuthorIds { get; set; }

    }
}
