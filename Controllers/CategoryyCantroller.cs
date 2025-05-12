//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using BoxBuildproj.Data;
//using BoxBuildproj.Models;
//using System.Linq;
//using System.Threading.Tasks;

//namespace BoxBuildproj.Controllers
//{
//    public class CategoryyController : Controller
//    {
//        private readonly BoxBuildprojContext _context;

//        public CategoryyController(BoxBuildprojContext context)
//        {
//            _context = context;
//        }

//        // GET: Cate (List all categories)
//        public async Task<IActionResult> Index()
//        {
//            var categories = await _context.Catgy.ToListAsync();
//            return View("~/Views/Cate/Index.cshtml", categories);
//        }

//        // GET: Cate/Create
//        public IActionResult Create()
//        {
//            return View("~/Views/Cate/Create.cshtml");
//        }

//        // POST: Cate/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(catgy category)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Catgy.Add(category);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View("~/Views/Cate/Create.cshtml", category);
//        }

//        // GET: Cate/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null) return NotFound();

//            var category = await _context.Catgy.FindAsync(id);
//            if (category == null) return NotFound();

//            return View("~/Views/Cate/Edit.cshtml", category);
//        }

//        // POST: Cate/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, catgy category)
//        {
//            if (id != category.CategoryId) return NotFound();

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Catgy.Update(category);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!_context.Catgy.Any(e => e.CategoryId == id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            return View("~/Views/Cate/Edit.cshtml", category);
//        }

//        // GET: Cate/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null) return NotFound();

//            var category = await _context.Catgy.FindAsync(id);
//            if (category == null) return NotFound();

//            return View("~/Views/Cate/Delete.cshtml", category);
//        }

//        // POST: Cate/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var category = await _context.Catgy.FindAsync(id);
//            if (category != null)
//            {
//                _context.Catgy.Remove(category);
//                await _context.SaveChangesAsync();
//            }
//            return RedirectToAction(nameof(Index));
//        }
//    }
//}
