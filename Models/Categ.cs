using System.ComponentModel.DataAnnotations;

namespace BoxBuildproj.Models
{
    public class Categ
    {
        [Key] // Primary Key
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        public string CategoryName { get; set; }
    }
}
