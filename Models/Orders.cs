using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BoxBuildproj.Areas.Identity.Data;

namespace BoxBuildproj.Models
{
    [Table("Orders")] // Optional: specify table name explicitly
    public class Orders
    {
        [Key]
        [Column("order_id")]
        public int OrderId { get; set; }

        [Required]
        [Column("user_id")]
        public string UserId { get; set; }

        [Required]
        [Column("total_price")]
        public decimal TotalPrice { get; set; }

        [Column("shipping_address")]
        public string ShippingAddress { get; set; }

        [Column("billing_address")]
        public string BillingAddress { get; set; }

        [Column("payment_status")]
        public string PaymentStatus { get; set; }

        [Column("order_status")]
        public string OrderStatus { get; set; }

        [ForeignKey("UserId")]
        public virtual BoxBuildprojUser User { get; set; }
    }
}

