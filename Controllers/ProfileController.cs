using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using BoxBuildproj.Data;
using BoxBuildproj.Models;
using BoxBuildproj.Areas.Identity.Data;
using BoxBuildproj.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BoxBuildproj.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly BoxBuildprojContext _context;
        private readonly UserManager<BoxBuildprojUser> _userManager;

        public ProfileController(BoxBuildprojContext context, UserManager<BoxBuildprojUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var cartItems = await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == user.Id)
                .ToListAsync();

            var wishlistItems = await _context.Wishlists
                .Include(w => w.Product)
                .Where(w => w.UserId == user.Id)
                .ToListAsync();

            var orders = await _context.Orders
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var viewModel = new UserProfileViewModel
            {
                User = user,
                CartItems = cartItems,
                WishlistItems = wishlistItems,
                Orders = orders
            };

            return View(viewModel);
        }
    }
}
