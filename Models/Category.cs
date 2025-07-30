using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace LibraryManagementBackend.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        [JsonIgnore]
         public List<Book>? Books { get; set; }
    }
}
