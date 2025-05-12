//using System.Diagnostics;
//using System.Security.Claims;
//using BoxBuildproj.Models;
//using Microsoft.AspNetCore.Mvc;


//namespace BoxBuildproj.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;

//        public HomeController(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }

//        public IActionResult Index()
//        {
//            return Redirect("/Identity/Account/Login");

//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get User ID
//            var userEmail = User.FindFirstValue(ClaimTypes.Email); // Get User Email

//            ViewData["UserId"] = userId;
//            ViewData["UserEmail"] = userEmail;

//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}


using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using BoxBuildproj.Models;

namespace BoxBuildproj.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Check if user is logged in
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login"); // Redirect if not logged in
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get User ID
            var userEmail = User.FindFirstValue(ClaimTypes.Email); // Get User Email

            ViewData["UserId"] = userId;
            ViewData["UserEmail"] = userEmail;

            return View(); // Return the view properly
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


