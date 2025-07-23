using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BoxBuildproj.Areas.Identity.Data;

namespace BoxBuildproj.Models
{
    public class RecentlyViewed
    {
        [Key]
        public int RecentlyViewedId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public DateTime ViewedAt { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual BoxBuildprojUser User { get; set; }

        [ForeignKey("ProductId")]
        public virtual Productstbl Productstbl { get; set; }
    }
}
