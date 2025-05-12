//using System.ComponentModel.DataAnnotations;

//namespace BoxBuildproj.Models
//{
//    public class Category
//    {
//        [Key]
//        public int CategoryId { get; set; }

//        [Required]
//        [StringLength(100)]
//        public string CategoryName { get; set; }
//    }
//}

using System.ComponentModel.DataAnnotations;

namespace BoxBuildproj.Models;
public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Required]
    public string CategoryName { get; set; }

    //public virtual ICollection<Product> Products { get; set; }
}
