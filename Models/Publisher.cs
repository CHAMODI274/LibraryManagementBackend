using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace LibraryManagementBackend.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }

        [JsonIgnore]
         public List<Book>? Books { get; set; }
    }
}
