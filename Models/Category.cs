using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;


namespace LibraryManagementBackend.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
         public List<Book>? Books { get; set; }
    }
}
