//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using BoxBuildproj.Models;
//using Microsoft.AspNetCore.Identity;
//public class Cart
//{
//    [Key]
//    public int CartId { get; set; }

//    // ForeignKey reference to IdentityUser
//    [ForeignKey("User")]
//    public string UserId { get; set; }  // Identity uses string GUID for UserId

//    [ForeignKey("Product")]
//    public int ProductId { get; set; }

//    public int Quantity { get; set; }

//    // Navigation properties
//    public virtual IdentityUser User { get; set; }
//    public virtual Product Product { get; set; }
//}
//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using BoxBuildproj.Areas.Identity.Data; // ✅ Import your custom user model
//using BoxBuildproj.Models;

//public class Cart
//{
//    [Key]
//    public int CartId { get; set; }

//    // Foreign Key reference to BoxBuildprojUser
//    [ForeignKey("User")]
//    public string UserId { get; set; }  // ✅ Identity uses string GUID for UserId

//    [ForeignKey("Product")]
//    public int ProductId { get; set; }

//    public int Quantity { get; set; }

//    // Navigation properties
//    public virtual BoxBuildprojUser User { get; set; } // ✅ FIXED: Now referencing the correct user class
//    public virtual Product Product { get; set; }
//}

//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;
//using BoxBuildproj.Areas.Identity.Data; // ✅ Import your custom user model
//using BoxBuildproj.Models;
//public class Cart
//{

//    [Key]
//    public int CartId { get; set; }

//    [ForeignKey("User")]
//    public required int userId { get; set; }
//    public virtual BoxBuildprojUser User { get; set; }

//    [ForeignKey("Product")]
//    public int ProductId { get; set; }
//    public virtual Product Product { get; set; }

//    public int Quantity { get; set; }
//}



using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BoxBuildproj.Areas.Identity.Data;

namespace BoxBuildproj.Models
{
    [Table("cart")] // 👈 Ensure correct table mapping
    public class Cart
    {
        [Key]
        [Column("cart_id")] // 👈 Match database column name
        public int CartId { get; set; }

        [Required]
        [Column("user_id")] // 👈 Match database column name
        public string UserId { get; set; }

        [Required]
        [Column("product_id")] // 👈 Match database column name
        public int ProductId { get; set; }

        [Required]
        [Column("quantity")] // 👈 Match database column name
        public int Quantity { get; set; }

        [ForeignKey("UserId")]
        public virtual BoxBuildprojUser User { get; set; }

        [ForeignKey("ProductId")]
        public virtual Productstbl Product { get; set; }
    }
}
