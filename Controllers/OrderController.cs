//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using BoxBuildproj.Data;
//using BoxBuildproj.Models;
//using BoxBuildproj.Models.ViewModels;
//using BoxBuildproj.Areas.Identity.Data;

//namespace BoxBuildproj.Controllers
//{
//    public class OrderController : Controller
//    {
//        private readonly BoxBuildprojContext _context;
//        private readonly UserManager<BoxBuildprojUser> _userManager;

//        public OrderController(BoxBuildprojContext context, UserManager<BoxBuildprojUser> userManager)
//        {
//            _context = context;
//            _userManager = userManager;
//        }

//        // GET: /Order/MyOrders

//        public async Task<IActionResult> MyOrders()
//        {
//            var user = await _userManager.GetUserAsync(User);
//            if (user == null)
//                return RedirectToAction("Login", "Account");

//            var userOrders = await _context.Orders
//                .Where(o => o.UserId == user.Id)
//                .Include(o => o.OrderDetails)              // Include OrderDetails
//                    .ThenInclude(od => od.Product)         // Include Product inside OrderDetails
//                .OrderByDescending(o => o.OrderDate)       // Optional: newest orders first
//                .ToListAsync();

//            return View(userOrders);
//        }
//        public async Task<IActionResult> Details(int id)
//        {
//            var user = await _userManager.GetUserAsync(User);
//            if (user == null)
//                return RedirectToAction("Login", "Account");

//            var order = await _context.Orders
//                .Include(o => o.OrderDetails)
//                    .ThenInclude(od => od.Product)
//                .FirstOrDefaultAsync(o => o.OrderId == id && o.UserId == user.Id);

//            if (order == null)
//                return NotFound();

//            var viewModel = new OrderDetailsViewModel
//            {
//                Order = order,
//                Items = order.OrderDetails.Select(od => new OrderItemDetail
//                {
//                    ProductName = od.Product.ProductName,
//                    ProductImage = od.Product.ImagePath, // optional
//                    Price = od.Product.Price,
//                    Quantity = od.Quantity
//                }).ToList()
//            };

//            return View(viewModel);
//        }


//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using BoxBuildproj.Data;
using BoxBuildproj.Models;
using BoxBuildproj.Models.ViewModels;
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
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(userOrders);
        }

        // GET: /Order/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id && o.UserId == user.Id);

            if (order == null)
                return NotFound();

            var viewModel = new OrderDetailsViewModel
            {
                Order = order,
                Items = order.OrderDetails.Select(od => new OrderItemDetail
                {
                    ProductName = od.Product.ProductName,
                    ProductImage = od.Product.ImagePath,
                    Price = od.Product.Price,
                    Quantity = od.Quantity
                }).ToList()
            };

            return View(viewModel);
        }
    }
}


//using BoxBuildproj.Data;
//using BoxBuildproj.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using BoxBuildproj.Models;
//public class OrderController : Controller
//{
//    private readonly BoxBuildprojContext _context;

//    public OrderController(BoxBuildprojContext context)
//    {
//        _context = context;
//    }

//    public async Task<IActionResult> MyOrders()
//    {
//        var userId = User.Identity.Name; // or however you store the user ID

//        var orders = await _context.Orders
//            .Where(o => o.UserId == userId)
//            .Include(o => o.Order)
//                .ThenInclude(od => od.Productstbl)
//            .ToListAsync();

//        var orderViewModels = orders.Select(o => new OrderViewModel
//        {
//            OrderId = o.OrderId,
//            OrderDate = o.OrderDate,
//            Status = o.Status,
//            TotalAmount = o.OrderDetails.Sum(d => d.Price * d.Quantity),
//            OrderItems = o.OrderDetails.Select(d => new OrderItemViewModel
//            {
//                ProductId = d.ProductId,
//                ProductName = d.Product.ProductName,
//                ImagePath = d.Product.ImagePath,
//                Price = d.Price,
//                Quantity = d.Quantity
//            }).ToList()
//        }).ToList();

//        return View(orderViewModels);
//    }
//}
