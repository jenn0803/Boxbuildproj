//using Razorpay.Api;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace BoxBuildproj.Models
//{
//    [Table("ProductOffer")]
//    public class ProductOffer
//    {
//        public int ProductOfferId { get; set; }

//        public int ProductId { get; set; }
//        public Product Product { get; set; }

//        public int OfferId { get; set; }
//        public Offer Offer { get; set; }
//    }

//}


using System.ComponentModel.DataAnnotations;

namespace BoxBuildproj.Models
{
    public class ProductOffer
    {
        [Key]
        public int ProductOfferId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int OfferId { get; set; }

        public Productstbl Product { get; set; }
        public Offer Offer { get; set; }
    }
}
