using BoxBuildproj.Data;
using Microsoft.AspNetCore.Mvc;
using BoxBuildproj.Data;
using BoxBuildproj.Models;

namespace YourNamespace.Controllers
{
    public class ProductDetailsController : Controller
    {
        private readonly BoxBuildprojContext _context;

        public ProductDetailsController(BoxBuildprojContext context)
        {
            _context = context;
        }

        // GET: ProductDetails/AddDetails
        public IActionResult AddDetails(int ProductID)
        {
            var model = new ProductDetails
            {
                ProductID = ProductID
            };
            return View(model);
        }


        // POST: ProductDetails/AddDetails
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDetails(ProductDetails detail)
        {
            if (ModelState.IsValid)
            {
                _context.ProductDetails.Add(detail);
                await _context.SaveChangesAsync();
                // After saving, redirect to product list or details
                return RedirectToAction("Index", "Products");
            }

            return View(detail);
        }
    }
}
