//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Threading.Tasks;
//using BoxBuildproj.Areas.Identity.Data;
//using BoxBuildproj.Models; // Adjust this based on where your Product and Category models are
//using BoxBuildproj.Data; // Update if your ApplicationDbContext is in a different namespace

//namespace BoxBuildproj.Controllers
//{
//    [Authorize(Roles = "Admin")]
//    public class AdminController : Controller
//    {
//        private readonly UserManager<BoxBuildprojUser> _userManager;
//        private readonly BoxBuildprojContext _context;

//        public AdminController(UserManager<BoxBuildprojUser> userManager, BoxBuildprojContext context)
//        {
//            _userManager = userManager;
//            _context = context;
//        }

//        public async Task<IActionResult> Index()
//        {
//            ViewData["TotalUsers"] = await _userManager.Users.CountAsync();
//            ViewData["TotalProducts"] = await _context.Productstbl.CountAsync();

//            return View();
//        }

//        public async Task<IActionResult> Userdisplay()
//        {
//            var users = await _userManager.Users.ToListAsync();
//            ViewData["TotalUsers"] = users.Count;
//            return View(users);
//        }

//        public IActionResult Profile() => View();
//        public IActionResult MailInbox() => View();
//        public IActionResult Home() => View();
//    }
//}


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using BoxBuildproj.Areas.Identity.Data;
using BoxBuildproj.Models;
using BoxBuildproj.Data;

namespace BoxBuildproj.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<BoxBuildprojUser> _userManager;
        private readonly BoxBuildprojContext _context;

        public AdminController(UserManager<BoxBuildprojUser> userManager, BoxBuildprojContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["TotalUsers"] = await _userManager.Users.CountAsync();
            ViewData["TotalProducts"] = await _context.Productstbl.CountAsync();

            return View();
        }

        public async Task<IActionResult> Home()
        {
            DateTime oneWeekAgo = DateTime.Now.AddDays(-7);

            int weeklyNewUsers = await _userManager.Users
                .Where(u => u.CreatedAt >= oneWeekAgo)
                .CountAsync();

            int weeklyProducts = await _context.Productstbl
                .Where(p => p.CreatedAt >= oneWeekAgo)
                .CountAsync();

            int weeklyOrders = await _context.Orders
                .Where(o => o.OrderDate >= oneWeekAgo)
                .CountAsync();

            ViewBag.WeeklyNewUsers = weeklyNewUsers;
            ViewBag.WeeklyProducts = weeklyProducts;
            ViewBag.WeeklyOrders = weeklyOrders;

            return View();
        }

        public async Task<IActionResult> Userdisplay()
        {
            var users = await _userManager.Users.ToListAsync();
            ViewData["TotalUsers"] = users.Count;
            return View(users);
        }

        public IActionResult Profile() => View();
        public IActionResult MailInbox() => View();
    }
}
