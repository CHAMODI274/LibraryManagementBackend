using System.Collections.Generic;

// This namespace provides the [JsonIgnore] attribute
// and other features for JSON serialization and deserialization
using System.Text.Json.Serialization;


namespace LibraryManagementBackend.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Bio { get; set; }



        // This list connects authors to books (an author can have many books).
        // But we don’t want to include this list in the API response
        // because it can cause too much data or circular reference issues.
        //
        // So we use [JsonIgnore] to tell the system:
        // “Don’t show this when sending JSON data to the frontend.”

        // [JsonIgnore] is an attribute used during JSON serialization.
        // Serialization means converting a C# object into JSON format
        // (for example, when returning data in an API response).
        //
        // This attribute tells the JSON serializer:
        // “Skip this property when generating the JSON.”
        [JsonIgnore]
        public List<BookAuthor>? BookAuthors { get; set; }
    }
}
