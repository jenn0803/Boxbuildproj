//using BoxBuildproj.Data;
//using Microsoft.AspNetCore.Mvc;
//using System.Linq;
//using User.Models;

//namespace User.Controllers  // Keep this if your controller is inside 'Controllers' folder
//{
//    public class UserController : Controller
//    {
//        private readonly BoxBuildprojContext _context;

//        public UserController(BoxBuildprojContext context)
//        {
//            _context = context;
//        }

//        public IActionResult Profile(int id)
//        {
//            //var user = new AppUser
//            {
//                FullName = "John Doe",
//                Email = "johndoe@example.com",
//                PhoneNumber = "123-456-7890",
//                Address = "123 Main Street, City"

//            };
//            return View(user);
//        }
//    }
//}

using BoxBuildproj.Data;
using BoxBuildproj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BoxBuildproj.Controllers
{
    public class UserController : Controller
    {
        private readonly BoxBuildprojContext _context;

        public UserController(BoxBuildprojContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Home()
        {
            // Step 1: Fetch OrderDetails + Product in memory
            var orderDetails = await _context.OrderDetails
                .Include(od => od.Product)
                .ToListAsync();

            // Step 2: Group & transform in memory
            var trending = orderDetails
                .Where(od => od.Product != null) // null check
                .GroupBy(od => od.ProductId)
                .Select(g =>
                {
                    var product = g.First().Product;

                    return new TrendingProductViewModel
                    {
                        ProductId = g.Key,
                        ProductName = product.ProductName,
                        Price = product.Price,
                        ImagePath = product.ImagePath ?? "default.jpg",
                        TotalSold = g.Sum(x => x.Quantity)
                    };
                })
                .OrderByDescending(p => p.TotalSold)
                .Take(6)
                .ToList();

            var allProducts = await _context.Productstbl.ToListAsync();

            var viewModel = new HomeViewModel
            {
                AllProducts = allProducts,
                TrendingProducts = trending
            };

            return View("Home", viewModel);
        }

        public IActionResult About()
        {
            return View("About"); // This will look for Views/User/About.cshtml
        }

        public IActionResult Contact()
        {
            return View("Contact");
        }

        public IActionResult Cart()
        {
            return View("Cart");
        }

        public IActionResult Checkout()
        {
            return View("Chekout");
        }
        public IActionResult Login()
        {
            return View("Login");
        }
        public async Task<ActionResult> Product_list()
        {
            var products = await _context.Productstbl.ToListAsync();
            return View("Product_list", products);
        }

        public async Task<ActionResult> Productsdetail()
        {
            return View("Productsdetail");
        }

        public IActionResult Products_list(string category = null)
        {
            // Fetch distinct categories from database
            var categories = _context.Productstbl
                .Select(p => p.Category)
                .Distinct()
                .ToList();

            ViewBag.Categories = categories;

            // Fetch products based on selected category
            var products = string.IsNullOrEmpty(category)
                ? _context.Productstbl.ToList()
                : _context.Productstbl.Where(p => p.Category == category).ToList();

            if (products == null || !products.Any())
            {
                products = new List<Productstbl>(); // Ensure Model is never null
            }

            return View("Product_list", products);
        }

      
    }
}