//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using BoxBuildproj.Models;
//using Microsoft.AspNetCore.Identity;

//[Table("Offer")]
//public class Offer
//{
//    public int OfferId { get; set; }

//    [Required]
//    public string Title { get; set; }

//    public string Description { get; set; }

//    [Required]
//    public decimal DiscountPercentage { get; set; }

//    public DateTime StartDate { get; set; }
//    public DateTime EndDate { get; set; }

//    public ICollection<ProductOffer> ProductOffers { get; set; }
//}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BoxBuildproj.Models;
using Microsoft.AspNetCore.Identity;


namespace BoxBuildproj.Models
{
    public class Offer
    {
        [Key]
        public int OfferId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        [Required]
        public decimal DiscountPercentage { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public ICollection<ProductOffer>? ProductOffers { get; set; }

        public ICollection<Productstbl>? Productstbl { get; set; }
    }
}
