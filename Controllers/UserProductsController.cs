using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BoxBuildproj.Models;
using BoxBuildproj.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;

namespace BoxBuildproj.Controllers
{
    public class UserProductsController : Controller
    {
        private readonly BoxBuildprojContext _context;

        public UserProductsController(BoxBuildprojContext context)
        {
            _context = context;
        }

        // GET: Home page with product list (if used)
        public async Task<IActionResult> Index()
        {
            var products = await _context.Productstbl.ToListAsync();
            return View("~/Views/User/Home.cshtml", products);
        }

        // GET: Product Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Productstbl
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> Product_list(string search, string category, decimal? minPrice, decimal? maxPrice)
        {
            var products = _context.Productstbl.AsQueryable();

            // Search by name or description
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.ProductName.Contains(search) || p.Description.Contains(search));
            }

            // Filter by category
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category == category);
            }

            // Filter by price range
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }

            // Populate dropdown filters
            ViewBag.Categories = await _context.Productstbl
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();

            // Keep selected filters
            ViewBag.Search = search;
            ViewBag.SelectedCategory = category;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;

            return View(await products.ToListAsync());
        }
    }
}
