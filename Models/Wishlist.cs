//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using BoxBuildproj.Models;
//using Microsoft.AspNetCore.Identity;

//public class Wishlist
//{
//    [Key]
//    public int WishlistId { get; set; }

//    [ForeignKey("AspNetUsers")]
//    public string Id { get; set; }

//    [ForeignKey("Product")]
//    public int ProductId { get; set; }

//    public virtual IdentityUser User { get; set; }
// //   public virtual Product Product { get; set; }
//}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BoxBuildproj.Areas.Identity.Data;

namespace BoxBuildproj.Models
{
    [Table("wishlist")] // 👈 Ensure correct table mapping
    public class Wishlist
    {
        [Key]
        [Column("wishlist_id")] // 👈 Match database column name
        public int WishlistId { get; set; }

        [Required]
        [Column("user_id")] // 👈 Match database column name
        public string UserId { get; set; }

        [Required]
        [Column("product_id")] // 👈 Match database column name
        public int ProductId { get; set; }

        [ForeignKey("UserId")]
        public virtual BoxBuildprojUser User { get; set; }

        [ForeignKey("ProductId")]
        public virtual Productstbl Product { get; set; }
    }
}
