using System.ComponentModel.DataAnnotations;

namespace LibraryManagementBackend.Models
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;

    }
}
