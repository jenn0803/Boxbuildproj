using System.Collections.Generic;
using BoxBuildproj.Models;
using BoxBuildproj.Areas.Identity.Data;

namespace BoxBuildproj.ViewModels
{
    public class UserProfileViewModel
    {
        public BoxBuildprojUser User { get; set; }

        public List<Cart> CartItems { get; set; }
        public List<Wishlist> WishlistItems { get; set; }
        public List<Orders> Orders { get; set; }
    }
}
