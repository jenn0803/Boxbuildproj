//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Http;
//using System.Collections.Generic;
//using System.Linq;
//using BoxBuilddbproj.Models;  // ✅ Ensure this namespace contains your Product model
//using User.Helpers;
//using BoxBuildproj.Data; // ✅ Import SessionExtensions for JSON session storage

//namespace BoxBuildproj.Controllers
//{
//    public class WishlistController : Controller
//    {
//        private readonly BoxBuildprojContext db;

//        // ✅ Inject database context into the constructor
//        public WishlistController(BoxBuildprojContext context)
//        {
//            db = context;
//        }

//        public IActionResult Index()
//        {
//            var wishlist = HttpContext.Session.GetObjectFromJson<List<Product>>("Wishlist") ?? new List<Product>();
//            return View(wishlist);
//        }

//        public IActionResult AddToWishlist(int id)
//        {
//            var product = db.Products.Find(id);
//            if (product != null)
//            {
//                var wishlist = HttpContext.Session.GetObjectFromJson<List<Product>>("Wishlist") ?? new List<Product>();

//                // ✅ Check if the product is already in the wishlist
//                if (!wishlist.Any(p => p.Id == id))
//                {
//                    wishlist.Add(product);
//                }

//                HttpContext.Session.SetObjectAsJson("Wishlist", wishlist);
//            }
//            return RedirectToAction("Index");
//        }

//        public IActionResult RemoveFromWishlist(int id)
//        {
//            var wishlist = HttpContext.Session.GetObjectFromJson<List<Product>>("Wishlist") ?? new List<Product>();
//            var itemToRemove = wishlist.FirstOrDefault(p => p.Id == id);

//            if (itemToRemove != null)
//            {
//                wishlist.Remove(itemToRemove);
//                HttpContext.Session.SetObjectAsJson("Wishlist", wishlist);
//            }
//            return RedirectToAction("Index");
//        }
//    }
//}


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BoxBuildproj.Data;
using BoxBuildproj.Models;

namespace BoxBuildproj.Controllers
{
    [Authorize] // 👈 Ensure user is logged in
    public class WishlistController : Controller
    {
        private readonly BoxBuildprojContext _context;

        public WishlistController(BoxBuildprojContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishlistItems = await _context.Wishlists
                .Where(w => w.UserId == userId)
                .Include(w => w.Product)
                .ToListAsync();

            return View(wishlistItems);
        }

        public async Task<IActionResult> AddToWishlist(int productId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingWishlistItem = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (existingWishlistItem == null)
            {
                var wishlistItem = new Wishlist
                {
                    UserId = userId,
                    ProductId = productId
                };
                _context.Wishlists.Add(wishlistItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveFromWishlist(int id)
        {
            var wishlistItem = await _context.Wishlists.FindAsync(id);
            if (wishlistItem != null)
            {
                _context.Wishlists.Remove(wishlistItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
