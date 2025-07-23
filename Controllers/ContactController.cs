using Microsoft.AspNetCore.Mvc;
using BoxBuildproj.Models;
using BoxBuildproj.Data;

namespace BoxBuildproj.Controllers
{
    public class ContactController : Controller
    {
        private readonly BoxBuildprojContext _context;

        public ContactController(BoxBuildprojContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Message sent successfully!";
                return RedirectToAction("Index");
            }

            return View(contact);
        }
    }
}
