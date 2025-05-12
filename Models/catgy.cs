using System.ComponentModel.DataAnnotations;

namespace BoxBuildproj.Models
{
    public class catgy
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }
        // Navigation Property: One Category -> Many Products
       // public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
