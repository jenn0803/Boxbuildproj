//using BoxBuildproj.Data;
//using BoxBuildproj.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace BoxBuildproj.Controllers
//{
//    public class OfferController : Controller
//    {
//        private readonly BoxBuildprojContext _context;

//        public OfferController(BoxBuildprojContext context)
//        {
//            _context = context;
//        }

//        // GET: Offer/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: Offer/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Create(Offer offer)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Offers.Add(offer);
//                _context.SaveChanges();
//                TempData["Success"] = "Offer created successfully!";
//                return RedirectToAction("Index", "Offer");
//            }
//            return View(offer);
//        }

//        // GET: Offer/AssignProducts/{offerId}
//        public IActionResult AssignProducts(int offerId)
//        {
//            var offer = _context.Offers.Find(offerId);
//            if (offer == null)
//            {
//                return NotFound();
//            }

//            var model = new OfferAssignViewModel
//            {
//                OfferId = offerId,
//                OfferTitle = offer.Title,
//                AllProducts = _context.Productstbl.ToList()
//            };

//            return View(model);
//        }

//        // POST: Offer/AssignProducts
//        [HttpPost]
//        public IActionResult AssignProducts(int offerId, int[] selectedProductIds)
//        {
//            foreach (var productId in selectedProductIds)
//            {
//                var exists = _context.ProductOffers.Any(po => po.ProductId == productId && po.OfferId == offerId);
//                if (!exists)
//                {
//                    _context.ProductOffers.Add(new ProductOffer
//                    {
//                        ProductId = productId,
//                        OfferId = offerId
//                    });
//                }
//            }

//            _context.SaveChanges();
//            TempData["Success"] = "Offer assigned to selected products!";
//            return RedirectToAction("Index", "Offer");
//        }

//        // GET: Offer/OfferProducts
//        public IActionResult OfferProducts()
//        {
//            var currentDate = DateTime.Now;
//            var offers = _context.ProductOffers
//                .Include(po => po.Product)
//                .Include(po => po.Offer)
//                .Where(po => po.Offer.StartDate <= currentDate && po.Offer.EndDate >= currentDate)
//                .ToList();

//            return View(offers);
//        }

//        // GET: Offer/Index
//        public IActionResult Index()
//        {
//            var offers = _context.Offers.ToList();
//            return View(offers);
//        }
//    }
//}


//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using BoxBuildproj.Data;
//using BoxBuildproj.Models;

//namespace BoxBuildproj.Controllers
//{
//    public class OfferController : Controller
//    {
//        private readonly BoxBuildprojContext _context;

//        public OfferController(BoxBuildprojContext context)
//        {
//            _context = context;
//        }

//        public async Task<IActionResult> Index()
//        {
//            return View(await _context.Offers.ToListAsync());
//        }

//        public IActionResult Create()
//        {
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(Offer offer)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(offer);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(offer);
//        }

//        public async Task<IActionResult> Edit(int id)
//        {
//            var offer = await _context.Offers.FindAsync(id);
//            if (offer == null) return NotFound();
//            return View(offer);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Edit(int id, Offer offer)
//        {
//            if (id != offer.OfferId) return NotFound();
//            if (ModelState.IsValid)
//            {
//                _context.Update(offer);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(offer);
//        }

//        public async Task<IActionResult> Delete(int id)
//        {
//            var offer = await _context.Offers.FindAsync(id);
//            if (offer == null) return NotFound();
//            return View(offer);
//        }

//        [HttpPost, ActionName("Delete")]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var offer = await _context.Offers.FindAsync(id);
//            _context.Offers.Remove(offer);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }
//    }
//}
