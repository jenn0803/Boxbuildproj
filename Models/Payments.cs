//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using BoxBuildproj.Models;
//using Microsoft.AspNetCore.Identity;

//public class Payments
//{
//    [Key]
//    public int PaymentId { get; set; }

//    [ForeignKey("Orders")]
//    public int OrderId { get; set; }

//    public string PaymentMethod { get; set; }
//    public string PaymentStatus { get; set; }
//    public DateTime PaymentDate { get; set; }
//    public decimal Amount { get; set; }

//    public virtual Orders Order { get; set; }
//}




using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxBuildproj.Models
{
    [Table("Payments")] // Matches the table name in SSMS
    public class Payments
    {
        [Key]
        [Column("payment_id")]
        public int PaymentId { get; set; }

        [Required]
        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("payment_method")]
        public string PaymentMethod { get; set; }

        [Column("payment_status")]
        public string PaymentStatus { get; set; }

        [Required]
        [Column("payment_date")]
        public DateTime PaymentDate { get; set; }

        [Required]
        [Column("amount")]
        public decimal Amount { get; set; }

        // Navigation property
        [ForeignKey("OrderId")]
        public virtual Orders Order { get; set; }
    }
}
