using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementBackend.Models
{
    public class Publisher
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Address { get; set; }

        [JsonIgnore]
         public List<Book>? Books { get; set; }
    }
}
