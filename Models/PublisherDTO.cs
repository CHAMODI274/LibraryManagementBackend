using System.ComponentModel.DataAnnotations;

namespace LibraryManagementBackend.Models
{
    public class PublisherDTO
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Address { get; set; }
    }
}
