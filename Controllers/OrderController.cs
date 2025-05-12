using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using BoxBuildproj.Data;
using BoxBuildproj.Models;
using BoxBuildproj.Areas.Identity.Data;

namespace BoxBuildproj.Controllers
{
    public class OrderController : Controller
    {
        private readonly BoxBuildprojContext _context;
        private readonly UserManager<BoxBuildprojUser> _userManager;

        public OrderController(BoxBuildprojContext context, UserManager<BoxBuildprojUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Order/MyOrders
        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var userOrders = await _context.Orders
                .Where(o => o.UserId == user.Id)
                .ToListAsync();

            return View(userOrders);
        }
    }
}
