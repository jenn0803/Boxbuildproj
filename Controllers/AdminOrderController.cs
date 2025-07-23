//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using BoxBuildproj.Data; // Your DbContext namespace
//using BoxBuildproj.Models; // Your Models namespace
//using System.Linq;
//using System.Threading.Tasks;

//public class AdminOrderController : Controller
//{
//    private readonly BoxBuildprojContext _context;

//    public AdminOrderController(BoxBuildprojContext context)
//    {
//        _context = context;
//    }

//    // GET: Orders (Admin view)
//    public async Task<IActionResult> Index()
//    {
//        // Fetch all orders with details and products eagerly loaded
//        var orders = await _context.Orders
//            .Include(o => o.OrderDetails)
//                .ThenInclude(od => od.Product)
//            .OrderByDescending(o => o.OrderDate)
//            .ToListAsync();

//        // Map to ViewModel
//        var orderViewModels = orders.Select(o => new OrderViewModel
//        {
//            OrderId = o.OrderId,
//            OrderDate = o.OrderDate,
//            CustomerName = o.CustomerName,
//            CustomerEmail = o.CustomerEmail,
//            TotalAmount = o.TotalPrice,
//            Status = o.OrderStatus,
//            OrderDetails = o.OrderDetails.Select(od => new OrderDetailViewModel
//            {
//                ProductName = od.Product.ProductName,
//                Quantity = od.Quantity,
//                Price = od.Price,
//                ImagePath = od.Product.ImagePath
//            }).ToList()
//        }).ToList();

//        return View(orderViewModels);
//    }
//}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BoxBuildproj.Data; // Your DbContext namespace
using BoxBuildproj.Models; // Your Models namespace
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using BoxBuildproj.Areas.Identity.Data; // For BoxBuildprojUser

namespace BoxBuildproj.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminOrderController : Controller
    {
        private readonly BoxBuildprojContext _context;

        public AdminOrderController(BoxBuildprojContext context)
        {
            _context = context;
        }

        // GET: Orders (Admin view)
           public async Task<IActionResult> Index()
        {
            // Fetch all orders with details, products, and user info
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.User) // Include user info
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            // Map to ViewModel
            var orderViewModels = orders.Select(o => new OrderViewModel
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                CustomerName = o.User?.UserName,
                CustomerEmail = o.User?.Email,
                TotalAmount = o.TotalPrice,
                Status = o.OrderStatus,

                // ✅ Set new values from the `Orders` entity
                ShippingAddress = o.ShippingAddress,
                PhoneNumber = o.PhoneNumber,

                OrderDetails = o.OrderDetails.Select(od => new OrderDetailViewModel
                {
                    ProductName = od.Product.ProductName,
                    Quantity = od.Quantity,
                    Price = od.Price,
                    ImagePath = od.Product.ImagePath
                }).ToList()
            }).ToList();

            // ✅ This line was missing
            return View(orderViewModels);
        }


 
        }
    }
