using System.ComponentModel.DataAnnotations;

namespace LibraryManagementBackend.Models
{
    public class AuthorDTO
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Bio { get; set; }

    }
}
