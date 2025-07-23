using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxBuildproj.Models
{
    [Table("Contact")]
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(200)]
        public string Subject { get; set; }

        public string Message { get; set; }

        public DateTime ContactDate { get; set; } = DateTime.Now;
    }
}
