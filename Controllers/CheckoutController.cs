//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;
//using System.Threading.Tasks;
//using System;
//using System.Collections.Generic;
//using BoxBuildproj.Data;
//using BoxBuildproj.Models;
//using Razorpay.Api;
//using BoxBuildproj.Areas.Identity.Data;

//namespace BoxBuildproj.Controllers
//{
//    public class CheckoutController : Controller
//    {
//        private readonly BoxBuildprojContext _context;
//        private readonly UserManager<BoxBuildprojUser> _userManager;

//        public CheckoutController(BoxBuildprojContext context, UserManager<BoxBuildprojUser> userManager)
//        {
//            _context = context;
//            _userManager = userManager;
//        }

//        // GET: /Checkout/
//        public async Task<IActionResult> Index()
//        {
//            var user = await _userManager.GetUserAsync(User);
//            if (user == null) return RedirectToAction("Login", "Account");

//            var cartItems = await _context.Carts
//                .Include(c => c.Product)
//                .Where(c => c.UserId == user.Id)
//                .ToListAsync();

//            decimal totalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity);

//            ViewBag.TotalAmount = totalAmount * 100; // In paise for Razorpay
//            ViewBag.UserEmail = user.Email;

//            return View(cartItems);
//        }

//        // POST: /Checkout/CreateOrder
//        [HttpPost]
//        public IActionResult CreateOrder(decimal totalAmount)
//        {
//            string key = "rzp_test_hXLdzaZ9MkR9Wf";
//            string secret = "KUh7eaRCk7l5dwuX5j41enwg";

//            RazorpayClient client = new RazorpayClient(key, secret);

//            var options = new Dictionary<string, object>
//            {
//                { "amount", totalAmount },     // In paise (₹500.00 = 50000)
//                { "currency", "INR" },
//                { "payment_capture", 1 }       // Auto capture
//            };

//            Order order = client.Order.Create(options);
//            return Json(new { orderId = order["id"].ToString() });
//        }

//        // GET: /Checkout/PaymentSuccess
//        public async Task<IActionResult> PaymentSuccess(string paymentId)
//        {
//            var user = await _userManager.GetUserAsync(User);
//            if (user == null) return RedirectToAction("Login", "Account");

//            var cartItems = await _context.Carts
//                .Include(c => c.Product)
//                .Where(c => c.UserId == user.Id)
//                .ToListAsync();

//            if (!cartItems.Any()) return RedirectToAction("Index", "Home");

//            // 1. Create Order
//            var order = new Orders
//            {
//                UserId = user.Id,
//                TotalPrice = cartItems.Sum(c => c.Product.Price * c.Quantity),
//                ShippingAddress = "Your shipping address here",
//                BillingAddress = "Your billing address here",
//                PaymentStatus = "Paid",
//                OrderStatus = "Placed"
//            };

//            _context.Orders.Add(order);
//            await _context.SaveChangesAsync(); // OrderId generated

//            // 2. Create Payment
//            var payment = new Payments
//            {
//                OrderId = order.OrderId,
//                Amount = order.TotalPrice,
//                PaymentDate = DateTime.Now,
//                PaymentMethod = "Razorpay",
//                PaymentStatus = "Success"
//            };

//            _context.Payments.Add(payment);

//            // 3. Clear Cart
//            _context.Carts.RemoveRange(cartItems);
//            await _context.SaveChangesAsync();

//            ViewBag.PaymentId = paymentId;
//            return View("Success");
//        }
//    }
//}



using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using BoxBuildproj.Data;
using BoxBuildproj.Models;
using Razorpay.Api;
using BoxBuildproj.Areas.Identity.Data;

namespace BoxBuildproj.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly BoxBuildprojContext _context;
        private readonly UserManager<BoxBuildprojUser> _userManager;

        // Razorpay test credentials (use appsettings in production)
        private const string RazorpayKey = "rzp_test_hXLdzaZ9MkR9Wf";
        private const string RazorpaySecret = "KUh7eaRCk7l5dwuX5j41enwg";

        public CheckoutController(BoxBuildprojContext context, UserManager<BoxBuildprojUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Checkout/
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var cartItems = await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == user.Id)
                .ToListAsync();

            decimal totalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity);

            ViewBag.TotalAmount = (int)(totalAmount * 100); // Convert to paise
            ViewBag.UserEmail = user.Email;
            ViewBag.RazorpayKey = RazorpayKey; // Send key to client for Razorpay JS

            return View(cartItems);
        }

        // POST: /Checkout/CreateOrder
        [HttpPost]
        public IActionResult CreateOrder([FromBody] RazorpayOrderRequest request)
        {
            try
            {
                if (request.TotalAmount < 1000) // Razorpay min is ₹10 = 1000 paise
                {
                    return BadRequest("Minimum amount must be ₹10.");
                }

                RazorpayClient client = new RazorpayClient(RazorpayKey, RazorpaySecret);

                var options = new Dictionary<string, object>
                {
                    { "amount", request.TotalAmount },
                    { "currency", "INR" },
                    { "payment_capture", 1 }
                };

                Order order = client.Order.Create(options);
                return Json(new { orderId = order["id"].ToString() });
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to create Razorpay order. " + ex.Message);
            }
        }

        // GET: /Checkout/PaymentSuccess
        public async Task<IActionResult> PaymentSuccess(string paymentId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var cartItems = await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == user.Id)
                .ToListAsync();

            if (!cartItems.Any()) return RedirectToAction("Index", "Home");

            var order = new Orders
            {
                UserId = user.Id,
                TotalPrice = cartItems.Sum(c => c.Product.Price * c.Quantity),
                ShippingAddress = "Your shipping address here",
                BillingAddress = "Your billing address here",
                PaymentStatus = "Paid",
                OrderStatus = "Placed"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var payment = new Payments
            {
                OrderId = order.OrderId,
                Amount = order.TotalPrice,
                PaymentDate = DateTime.Now,
                PaymentMethod = "Razorpay",
                PaymentStatus = "Success"
            };

            _context.Payments.Add(payment);

            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            ViewBag.PaymentId = paymentId;
            return View("Success");
        }
    }

    // DTO for Razorpay order creation
    public class RazorpayOrderRequest
    {
        public int TotalAmount { get; set; } // Amount in paise
    }
}
