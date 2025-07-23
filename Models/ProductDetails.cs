using BoxBuildproj.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace BoxBuildproj.Models
{
    public class ProductDetails
    {
        [Key]
        public int ProductDetailsId { get; set; }

        [ForeignKey("Productstbl")]
        public int ProductID { get; set; }

        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public string Material { get; set; }
        public string Color { get; set; }
        public string Thickness { get; set; }
        public decimal? WeightCapacity { get; set; }

        public Productstbl Product { get; set; }
    }
}