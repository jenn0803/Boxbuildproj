//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using BoxBuildproj.Models;
//using System;
//using BoxBuildproj.Data;

//public class AdminOffersController : Controller
//{
//    private readonly BoxBuildprojContext _context;

//    public AdminOffersController(BoxBuildprojContext context)
//    {
//        _context = context;
//    }

//    public IActionResult Index()
//    {
//        var offers = _context.Offer.ToList();
//        return View(offers);
//    }

//    public IActionResult Create()
//    {
//        return View();
//    }

//    [HttpPost]
//[ValidateAntiForgeryToken]
//    public IActionResult Create(Offer offer)
//    {
//        if (ModelState.IsValid)
//        {
//            // ✅ Ensure date range is valid before saving
//            if (offer.StartDate < new DateTime(1753, 1, 1))
//                offer.StartDate = DateTime.Now;

//            if (offer.EndDate < new DateTime(1753, 1, 1))
//                offer.EndDate = DateTime.Now.AddDays(7);

//            _context.Offer.Add(offer);
//            _context.SaveChanges();
//            return RedirectToAction(nameof(Index));
//        }

//        return View(offer);
//    }

//    public IActionResult AssignOffer()
//    {
//        ViewBag.Products = _context.Productstbl.ToList();
//        ViewBag.Offers = _context.Offer.ToList();
//        return View();
//    }

//    [HttpPost]
//    public IActionResult AssignOffer(int productId, int offerId)
//    {
//        var existing = _context.ProductOffer.FirstOrDefault(po => po.ProductId == productId && po.OfferId == offerId);
//        if (existing == null)
//        {
//            var productOffer = new ProductOffer
//            {
//                ProductId = productId,
//                OfferId = offerId
//            };
//            _context.ProductOffer.Add(productOffer);
//            _context.SaveChanges();
//        }

//        return RedirectToAction("Index");
//    }

//    public ActionResult Details(int id)
//    {
//        var offer = _context.Offer.Find(id);
//        if (offer == null)
//        {
//            return NotFound();
//        }
//        return View(offer);
//    }

//    protected override void Dispose(bool disposing)
//    {
//        if (disposing)
//        {
//            _context.Dispose();
//        }
//        base.Dispose(disposing);
//    }
//}




using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BoxBuildproj.Data;
using BoxBuildproj.Models;
using Microsoft.AspNetCore.Authorization;

namespace BoxBuildproj.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminOffersController : Controller
    {
        private readonly BoxBuildprojContext _context;

        public AdminOffersController(BoxBuildprojContext context)
        {
            _context = context;
        }

        // GET: AdminOffers
        public IActionResult Index()
        {
            var offers = _context.Offer.ToList();
            return View(offers);
        }

        // GET: AdminOffers/Details/5
        public IActionResult Details(int id)
        {
            var offer = _context.Offer.Find(id);
            if (offer == null) return NotFound();
            return View(offer);
        }

        // GET: AdminOffers/Create
        public IActionResult Create()
            => View();

        // POST: AdminOffers/Create
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Offer offer)
        {
            if (!ModelState.IsValid) return View(offer);

            // ensure sensible defaults
            if (offer.StartDate < DateTime.Now) offer.StartDate = DateTime.Now;
            if (offer.EndDate <= offer.StartDate) offer.EndDate = offer.StartDate.AddDays(7);

            _context.Offer.Add(offer);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: AdminOffers/Edit/5
        public IActionResult Edit(int id)
        {
            var offer = _context.Offer.Find(id);
            if (offer == null) return NotFound();
            return View(offer);
        }

        // POST: AdminOffers/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Offer updated)
        {
            if (id != updated.OfferId) return BadRequest();

            if (!ModelState.IsValid) return View(updated);

            // ensure date logic
            if (updated.EndDate <= updated.StartDate)
                ModelState.AddModelError(nameof(updated.EndDate), "End date must be after start date.");

            if (!ModelState.IsValid) return View(updated);

            try
            {
                _context.Entry(updated).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Offer.Any(o => o.OfferId == id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: AdminOffers/Delete/5
        public IActionResult Delete(int id)
        {
            var offer = _context.Offer.Find(id);
            if (offer == null) return NotFound();
            return View(offer);
        }

        // POST: AdminOffers/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var offer = _context.Offer.Include(o => o.ProductOffers).FirstOrDefault(o => o.OfferId == id);
            if (offer == null) return NotFound();

            // first remove any relationships
            if (offer.ProductOffers != null)
                _context.ProductOffer.RemoveRange(offer.ProductOffers);

            _context.Offer.Remove(offer);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: AdminOffers/AssignOffer
        public IActionResult AssignOffer()
        {
            ViewBag.Products = _context.Productstbl.ToList();
            ViewBag.Offers = _context.Offer.ToList();
            return View();
        }

        // POST: AdminOffers/AssignOffer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AssignOffer(int productId, int offerId)
        {
            var exists = _context.ProductOffer
                .FirstOrDefault(po => po.ProductId == productId && po.OfferId == offerId);
            if (exists == null)
            {
                _context.ProductOffer.Add(new ProductOffer
                {
                    ProductId = productId,
                    OfferId = offerId
                });
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _context.Dispose();
            base.Dispose(disposing);
        }
    }
}
