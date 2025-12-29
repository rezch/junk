using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace lab5.Models
{
    public class Student
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonPropertyName("studentName")]
        [Required(ErrorMessage = "name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "name must be between 2 and 20 characters long.")]
        public string Name { get; set; }

        [JsonPropertyName("studentAge")]
        [Required(ErrorMessage = "age is required")]
        public int Age { get; set; }
    }
}