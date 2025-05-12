using System.ComponentModel.DataAnnotations;

namespace BoxBuildproj.Models
{
    public class Categorytbl
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(255)]
        public string CategoryName { get; set; }
    }
}
