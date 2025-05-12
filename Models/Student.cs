using System.ComponentModel.DataAnnotations;

namespace BoxBuildproj.Models
{
    public class Student
    {
        [Key] // Primary Key
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(1, 100, ErrorMessage = "Age must be between 1 and 100")]
        public int Age { get; set; }
    }
}
