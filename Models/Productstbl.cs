using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BoxBuildproj.Models
{

    public class Productstbl
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        [StringLength(255)]
        public string ProductName { get; set; }

        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [Required]
        [StringLength(100)]
        public string Category { get; set; }

        public string? ImagePath { get; set; }  // New property
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [NotMapped]
        public decimal DiscountedPrice
        {
            get
            {
                var activeOffer = ProductOffer?
                    .FirstOrDefault(po => po.Offer != null &&
                                          po.Offer.StartDate <= DateTime.Now &&
                                          po.Offer.EndDate >= DateTime.Now);

                if (activeOffer != null)
                {
                    var discount = Price * activeOffer.Offer.DiscountPercentage / 100;
                    return Price - discount;
                }

                return Price;
            }
        }


        public ICollection<ProductOffer> ProductOffer { get; set; } = new List<ProductOffer>();
        public ICollection<Offer> Offer { get; set; } = new List<Offer>();
        public ICollection<ProductDetails> ProductDetails { get; set; } = new List<ProductDetails>();
    }
    }
