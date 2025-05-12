using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BoxBuildproj.Models;
using BoxBuildproj.Data;
using Microsoft.AspNetCore.Authorization;

namespace BoxBuildproj.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly BoxBuildprojContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductsController(BoxBuildprojContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Productstbl.ToListAsync());
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Productstbl product, IFormFile? ProductImage)
        {
            if (ModelState.IsValid)
            {
                // Debug: Check if file is received
                if (ProductImage != null && ProductImage.Length > 0)
                {
                    Console.WriteLine("ProductImage received:");
                    Console.WriteLine($"FileName: {ProductImage.FileName}");
                    Console.WriteLine($"FileLength: {ProductImage.Length}");

                    // Build the path for the uploads folder
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                        Console.WriteLine("Created 'images' folder.");
                    }

                    // Generate a unique file name
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ProductImage.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the file to the folder
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProductImage.CopyToAsync(fileStream);
                    }

                    // Save the relative path to the database
                    product.ImagePath = "/images/" + uniqueFileName;
                    Console.WriteLine($"ImagePath set to: {product.ImagePath}");
                }
                else
                {
                    Console.WriteLine("ProductImage is null or empty.");
                }

                _context.Productstbl.Add(product);
                await _context.SaveChangesAsync();
                Console.WriteLine("Product saved with ImagePath: " + product.ImagePath);

                return RedirectToAction(nameof(Index));
            }

            Console.WriteLine("Model state is invalid.");
            return View(product);
        }


        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.Productstbl.FindAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        //// POST: Products/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, Productstbl product, IFormFile? ProductImage)
        //{
        //    if (id != product.ProductID) return NotFound();

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (ProductImage != null)
        //            {
        //                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
        //                if (!Directory.Exists(uploadsFolder))
        //                {
        //                    Directory.CreateDirectory(uploadsFolder);
        //                }

        //                string uniqueFileName = Guid.NewGuid().ToString() + "_" + ProductImage.FileName;
        //                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //                using (var fileStream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await ProductImage.CopyToAsync(fileStream);
        //                }

        //                // Update the ImagePath only if a new image is uploaded
        //                product.ImagePath = "/images/" + uniqueFileName;
        //            }

        //            _context.Update(product);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!_context.Productstbl.Any(e => e.ProductID == id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(product);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Productstbl product, IFormFile? ProductImage)
        {
            if (id != product.ProductID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _context.Productstbl.AsNoTracking()
                                                .FirstOrDefaultAsync(p => p.ProductID == id);

                    if (existingProduct == null)
                        return NotFound();

                    // If new image is uploaded, update path
                    if (ProductImage != null)
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + ProductImage.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ProductImage.CopyToAsync(fileStream);
                        }

                        product.ImagePath = "/images/" + uniqueFileName;
                    }
                    else
                    {
                        // Preserve existing image path
                        product.ImagePath = existingProduct.ImagePath;
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Productstbl.Any(e => e.ProductID == id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.Productstbl.FindAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Productstbl
                                .FirstOrDefaultAsync(p => p.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Productstbl.FindAsync(id);
            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    string filePath = Path.Combine(_hostingEnvironment.WebRootPath, product.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Productstbl.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}