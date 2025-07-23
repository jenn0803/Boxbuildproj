//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using BoxBuildproj.Models;
//using BoxBuildproj.Data;
//using System.Threading.Tasks;
//using System.Linq;
//using System.Security.Claims;

//namespace BoxBuildproj.Controllers
//{
//    public class UserProductsController : Controller
//    {
//        private readonly BoxBuildprojContext _context;

//        public UserProductsController(BoxBuildprojContext context)
//        {
//            _context = context;
//        }

//        // GET: Home page with product list (if used)
//        public async Task<IActionResult> Index()
//        {
//            var products = await _context.Productstbl.ToListAsync();
//            return View("~/Views/User/Home.cshtml", products);
//        }

//        // GET: Product Details
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var product = await _context.Productstbl
//                .FirstOrDefaultAsync(m => m.ProductID == id);
//            if (product == null)
//            {
//                return NotFound();
//            }

//            return View(product);
//        }

//        public async Task<IActionResult> Product_list(string search, string category, decimal? minPrice, decimal? maxPrice)
//        {
//            var products = _context.Productstbl.AsQueryable();

//            // Search by name or description
//            if (!string.IsNullOrEmpty(search))
//            {
//                products = products.Where(p => p.ProductName.Contains(search) || p.Description.Contains(search));
//            }

//            // Filter by category
//            if (!string.IsNullOrEmpty(category))
//            {
//                products = products.Where(p => p.Category == category);
//            }

//            // Filter by price range
//            if (minPrice.HasValue)
//            {
//                products = products.Where(p => p.Price >= minPrice.Value);
//            }

//            if (maxPrice.HasValue)
//            {
//                products = products.Where(p => p.Price <= maxPrice.Value);
//            }

//            // Populate dropdown filters
//            ViewBag.Categories = await _context.Productstbl
//                .Select(p => p.Category)
//                .Distinct()
//                .ToListAsync();

//            // Keep selected filters
//            ViewBag.Search = search;
//            ViewBag.SelectedCategory = category;
//            ViewBag.MinPrice = minPrice;
//            ViewBag.MaxPrice = maxPrice;

//            return View(await products.ToListAsync());
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BoxBuildproj.Models;
using BoxBuildproj.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Razorpay.Api;
using BoxBuildproj.Areas.Identity.Data;

namespace BoxBuildproj.Controllers
{
    public class UserProductsController : Controller
    {
        private readonly BoxBuildprojContext _context;
        private readonly UserManager<BoxBuildprojUser> _userManager;

        public UserProductsController(BoxBuildprojContext context, UserManager<BoxBuildprojUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Home page (optional)
        public async Task<IActionResult> Index()
        {
            var products = await _context.Productstbl
                .Include(p => p.ProductOffer)
                    .ThenInclude(po => po.Offer)
                .ToListAsync();

            return View("~/Views/User/Home.cshtml", products);
        }

        // Product Details
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Productstbl
                .Include(p => p.ProductOffer)
                    .ThenInclude(po => po.Offer)
                .FirstOrDefaultAsync(p => p.ProductID == id);

            if (product == null)
                return NotFound();

            // For logged-in users (keep your existing logic here)
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var existing = await _context.RecentlyViewed
                    .FirstOrDefaultAsync(rv => rv.UserId == user.Id && rv.ProductId == id);

                if (existing == null)
                {
                    _context.RecentlyViewed.Add(new RecentlyViewed
                    {
                        UserId = user.Id,
                        ProductId = id,
                        ViewedAt = DateTime.Now
                    });
                }
                else
                {
                    existing.ViewedAt = DateTime.Now;
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                // Not logged in - Use cookies
                const string cookieKey = "recentlyViewed";
                var cookie = Request.Cookies[cookieKey];
                List<int> recentIds = new List<int>();

                if (!string.IsNullOrEmpty(cookie))
                {
                    recentIds = cookie.Split(',').Select(int.Parse).ToList();
                }

                if (!recentIds.Contains(id))
                {
                    recentIds.Insert(0, id); // Add to front
                    if (recentIds.Count > 5)
                        recentIds = recentIds.Take(5).ToList(); // Limit to 5 items

                    Response.Cookies.Append(cookieKey, string.Join(",", recentIds), new CookieOptions
                    {
                        Expires = DateTimeOffset.Now.AddDays(7)
                    });
                }
            }

            // Fetch recently viewed products
            List<Productstbl> recentlyViewed = new List<Productstbl>();

            if (user != null)
            {
                recentlyViewed = await _context.RecentlyViewed
                    .Where(rv => rv.UserId == user.Id && rv.ProductId != id)
                    .OrderByDescending(rv => rv.ViewedAt)
                    .Select(rv => rv.Productstbl)
                    .Take(5)
                    .ToListAsync();
            }
            else
            {
                const string cookieKey = "recentlyViewed";
                var cookie = Request.Cookies[cookieKey];
                if (!string.IsNullOrEmpty(cookie))
                {
                    var ids = cookie.Split(',').Select(int.Parse).Where(pid => pid != id).Take(5).ToList();
                    recentlyViewed = await _context.Productstbl
                        .Where(p => ids.Contains(p.ProductID))
                        .ToListAsync();
                }
            }

            ViewBag.RecentlyViewed = recentlyViewed;

            return View(product);
        }


        // Product List with Search, Filter, and Offers
        public async Task<IActionResult> Product_list(string search, string category, decimal? minPrice, decimal? maxPrice)
        {
            var productsQuery = _context.Productstbl
                .Include(p => p.ProductOffer)
                    .ThenInclude(po => po.Offer)
                .AsQueryable();

            // Search
            if (!string.IsNullOrEmpty(search))
            {
                productsQuery = productsQuery.Where(p => p.ProductName.Contains(search) || p.Description.Contains(search));
            }

            // Category filter
            if (!string.IsNullOrEmpty(category))
            {
                productsQuery = productsQuery.Where(p => p.Category == category);
            }

            // Price filters
            if (minPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price <= maxPrice.Value);
            }

            // Send categories to View
            ViewBag.Categories = await _context.Productstbl
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();

            // Retain filter values
            ViewBag.Search = search;
            ViewBag.SelectedCategory = category;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;

            return View(await productsQuery.ToListAsync());
        }
    }
}
