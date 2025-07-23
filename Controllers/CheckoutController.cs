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

//        // Razorpay test credentials (use appsettings in production)
//        private const string RazorpayKey = "rzp_test_hXLdzaZ9MkR9Wf";
//        private const string RazorpaySecret = "KUh7eaRCk7l5dwuX5j41enwg";

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

//            ViewBag.TotalAmount = (int)(totalAmount * 100); // Convert to paise
//            ViewBag.UserEmail = user.Email;
//            ViewBag.RazorpayKey = RazorpayKey; // Send key to client for Razorpay JS

//            return View(cartItems);
//        }

//        // POST: /Checkout/CreateOrder
//        [HttpPost]
//        public IActionResult CreateOrder([FromBody] RazorpayOrderRequest request)
//        {
//            try
//            {
//                if (request.TotalAmount < 1000) // Razorpay min is ₹10 = 1000 paise
//                {
//                    return BadRequest("Minimum amount must be ₹10.");
//                }

//                RazorpayClient client = new RazorpayClient(RazorpayKey, RazorpaySecret);

//                var options = new Dictionary<string, object>
//                {
//                    { "amount", request.TotalAmount },
//                    { "currency", "INR" },
//                    { "payment_capture", 1 }
//                };

//                Order order = client.Order.Create(options);
//                return Json(new { orderId = order["id"].ToString() });
//            }
//            catch (Exception ex)
//            {
//                return BadRequest("Failed to create Razorpay order. " + ex.Message);
//            }
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
//            await _context.SaveChangesAsync();

//            var payment = new Payments
//            {
//                OrderId = order.OrderId,
//                Amount = order.TotalPrice,
//                PaymentDate = DateTime.Now,
//                PaymentMethod = "Razorpay",
//                PaymentStatus = "Success"
//            };

//            _context.Payments.Add(payment);

//            _context.Carts.RemoveRange(cartItems);
//            await _context.SaveChangesAsync();

//            ViewBag.PaymentId = paymentId;
//            return View("Success");
//        }
//    }

//    // DTO for Razorpay order creation
//    public class RazorpayOrderRequest
//    {
//        public int TotalAmount { get; set; } // Amount in paise
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
using System.Net.Mail;
using System.Net;
using BoxBuildproj.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;


namespace BoxBuildproj.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly BoxBuildprojContext _context;
        private readonly UserManager<BoxBuildprojUser> _userManager;

        // Razorpay test credentials (store in appsettings for production)
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
                    .ThenInclude(p => p.ProductOffer)
                        .ThenInclude(po => po.Offer)
                .Where(c => c.UserId == user.Id)
                .ToListAsync();

            decimal totalAmount = 0;

            foreach (var item in cartItems)
            {
                decimal price = item.Product.Price;
                var offer = item.Product.ProductOffer?.FirstOrDefault()?.Offer;

                if (offer != null && offer.StartDate <= DateTime.Now && offer.EndDate >= DateTime.Now)
                {
                    price -= (offer.DiscountPercentage / 100m) * price;
                }

                totalAmount += price * item.Quantity;
            }

            ViewBag.TotalAmount = (int)(totalAmount * 100); // Convert to paise
            ViewBag.UserEmail = user.Email;
            ViewBag.RazorpayKey = RazorpayKey;

            return View(cartItems);
        }


        // POST: /Checkout/CreateOrder
        //[HttpPost]
        //public IActionResult CreateOrder([FromBody] RazorpayOrderRequest request)
        //{
        //    try
        //    {
        //        if (request.TotalAmount < 1000) // Razorpay minimum = ₹10 = 1000 paise
        //        {
        //            return BadRequest("Minimum amount must be ₹10.");
        //        }

        //        RazorpayClient client = new RazorpayClient(RazorpayKey, RazorpaySecret);

        //        var options = new Dictionary<string, object>
        //        {
        //            { "amount", request.TotalAmount },
        //            { "currency", "INR" },
        //            { "payment_capture", 1 }
        //        };

        //        Order order = client.Order.Create(options);
        //        return Json(new { orderId = order["id"].ToString() });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest("Failed to create Razorpay order. " + ex.Message);
        //    }
        //}
        [HttpPost]
        public IActionResult CreateOrder([FromBody] CheckoutData data)
        {
            try
            {
                if (data.TotalAmount < 1000) // Razorpay minimum = ₹10 = 1000 paise
                {
                    return BadRequest("Minimum amount must be ₹10.");
                }

                // Store in TempData (or Session if you prefer)
                TempData["ShippingAddress"] = data.Address;
                TempData["BillingAddress"] = data.Address;
                TempData["Phone"] = data.Phone;

                RazorpayClient client = new RazorpayClient(RazorpayKey, RazorpaySecret);

                var options = new Dictionary<string, object>
        {
            { "amount", data.TotalAmount },
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
        //    public async Task<IActionResult> PaymentSuccess(string paymentId)
        //    {
        //        var user = await _userManager.GetUserAsync(User);
        //        if (user == null) return RedirectToAction("Login", "Account");

        //        var cartItems = await _context.Carts
        //            .Include(c => c.Product)
        //                .ThenInclude(p => p.ProductOffer)
        //                    .ThenInclude(po => po.Offer)
        //            .Where(c => c.UserId == user.Id)
        //            .ToListAsync();

        //        if (!cartItems.Any()) return RedirectToAction("Index", "Home");

        //        decimal totalOrderPrice = 0;

        //        var order = new Orders
        //        {
        //            UserId = user.Id,
        //            ShippingAddress = "Your shipping address here",
        //            BillingAddress = "Your billing address here",
        //            PaymentStatus = "Paid",
        //            OrderStatus = "Placed",
        //            OrderDate = DateTime.Now
        
        //        };

        //        _context.Orders.Add(order);
        //        await _context.SaveChangesAsync();

        //        foreach (var item in cartItems)
        //        {
        //            decimal price = item.Product.Price;
        //            var offer = item.Product.ProductOffer?.FirstOrDefault()?.Offer;

        //            if (offer != null && offer.StartDate <= DateTime.Now && offer.EndDate >= DateTime.Now)
        //            {
        //                price -= (offer.DiscountPercentage / 100m) * price;
        //            }

        //            totalOrderPrice += price * item.Quantity;

        //            var orderDetails = new OrderDetails
        //            {
        //                OrderId = order.OrderId,
        //                ProductId = item.ProductId,
        //                Quantity = item.Quantity,
        //                Price = price
        //            };
        //            _context.OrderDetails.Add(orderDetails);
        //        }

        //        order.TotalPrice = totalOrderPrice;
        //        _context.Orders.Update(order);
        //        await _context.SaveChangesAsync();

        //        var payment = new Payments
        //        {
        //            OrderId = order.OrderId,
        //            Amount = totalOrderPrice,
        //            PaymentDate = DateTime.Now,
        //            PaymentMethod = "Razorpay",
        //            PaymentStatus = "Success"
        //        };
        //        _context.Payments.Add(payment);

        //        _context.Carts.RemoveRange(cartItems);
        //        await _context.SaveChangesAsync();

        //        // ✅ Prepare receipt HTML
        //        string receiptHtml = $@"
        //<h2 style='color:#4CAF50;'>Payment Receipt</h2>
        //<p><strong>Order ID:</strong> {order.OrderId}</p>
        //<p><strong>Payment ID:</strong> {paymentId}</p>
        //<p><strong>Name:</strong> {user.UserName}</p>
        //<p><strong>Email:</strong> {user.Email}</p>
        //<p><strong>Date:</strong> {DateTime.Now:dd/MM/yyyy hh:mm tt}</p>

        //<table style='width:100%; border-collapse:collapse; margin-top:20px;'>
        //    <thead>
        //        <tr style='background:#f2f2f2;'>
        //            <th style='border:1px solid #ddd; padding:8px;'>Product</th>
        //            <th style='border:1px solid #ddd; padding:8px;'>Qty</th>
        //            <th style='border:1px solid #ddd; padding:8px;'>Price (₹)</th>
        //            <th style='border:1px solid #ddd; padding:8px;'>Subtotal (₹)</th>
        //        </tr>
        //    </thead>
        //    <tbody>";

        //        foreach (var item in cartItems)
        //        {
        //            decimal unitPrice = item.Product.Price;
        //            var offer = item.Product.ProductOffer?.FirstOrDefault()?.Offer;
        //            if (offer != null && offer.StartDate <= DateTime.Now && offer.EndDate >= DateTime.Now)
        //            {
        //                unitPrice -= (offer.DiscountPercentage / 100m) * unitPrice;
        //            }
        //            var subtotal = unitPrice * item.Quantity;

        //            receiptHtml += $@"
        //        <tr>
        //            <td style='border:1px solid #ddd; padding:8px;'>{item.Product.ProductName}</td>
        //            <td style='border:1px solid #ddd; padding:8px;'>{item.Quantity}</td>
        //            <td style='border:1px solid #ddd; padding:8px;'>{unitPrice:F2}</td>
        //            <td style='border:1px solid #ddd; padding:8px;'>{subtotal:F2}</td>
        //        </tr>";
        //        }

        //        receiptHtml += $@"
        //    </tbody>
        //</table>
        //<p style='margin-top:20px; font-size:16px;'><strong>Total Paid:</strong> ₹{totalOrderPrice:F2}</p>
        //<p style='margin-top:20px;'>Thank you for shopping with BoxBuild! We hope you enjoy your order.</p>";

        //        // ✅ Send receipt via email only
        //        SendReceiptEmail(user.Email, "BoxBuild Payment Receipt", receiptHtml);

        //        // ✅ Show minimal success view
        //        ViewBag.Message = "Thank you! Your payment was successful. A detailed receipt has been sent to your email.";
        //        return View("Success");
        //    }

        public async Task<IActionResult> PaymentSuccess(string paymentId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var shippingAddress = TempData["ShippingAddress"] as string ?? "Not Provided";
            var billingAddress = TempData["BillingAddress"] as string ?? "Not Provided";
            var phone = TempData["Phone"] as string ?? "Not Provided";

            var cartItems = await _context.Carts
                .Include(c => c.Product)
                    .ThenInclude(p => p.ProductOffer)
                        .ThenInclude(po => po.Offer)
                .Where(c => c.UserId == user.Id)
                .ToListAsync();

            if (!cartItems.Any()) return RedirectToAction("Index", "Home");

            decimal totalOrderPrice = 0;

            var order = new Orders
            {
                UserId = user.Id,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                PaymentStatus = "Paid",
                OrderStatus = "Placed",
                OrderDate = DateTime.Now,
                PhoneNumber = phone
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in cartItems)
            {
                decimal price = item.Product.Price;
                var offer = item.Product.ProductOffer?.FirstOrDefault()?.Offer;

                if (offer != null && offer.StartDate <= DateTime.Now && offer.EndDate >= DateTime.Now)
                {
                    price -= (offer.DiscountPercentage / 100m) * price;
                }

                totalOrderPrice += price * item.Quantity;

                var orderDetails = new OrderDetails
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = price,
                   
                };
                _context.OrderDetails.Add(orderDetails);
            }

            order.TotalPrice = totalOrderPrice;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            var payment = new Payments
            {
                OrderId = order.OrderId,
                Amount = totalOrderPrice,
                PaymentDate = DateTime.Now,
                PaymentMethod = "Razorpay",
                PaymentStatus = "Success"
            };
            _context.Payments.Add(payment);

            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

           // Build receipt including phone and address

                    // ✅ Prepare receipt HTML
                    string receiptHtml = $@"
            <h2 style='color:#4CAF50;'>Payment Receipt</h2>
            <p><strong>Payment ID:</strong> {paymentId}</p>
    <p><strong>Name:</strong> {user.UserName}</p>
    <p><strong>Email:</strong> {user.Email}</p>
    <p><strong>Phone:</strong> {phone}</p>
    <p><strong>Shipping Address:</strong> {shippingAddress}</p>
    <p><strong>Billing Address:</strong> {billingAddress}</p>
    <p><strong>Date:</strong> {DateTime.Now:dd/MM/yyyy hh:mm tt}</p>

            <table style='width:100%; border-collapse:collapse; margin-top:20px;'>
                <thead>
                    <tr style='background:#f2f2f2;'>
                        <th style='border:1px solid #ddd; padding:8px;'>Product</th>
                        <th style='border:1px solid #ddd; padding:8px;'>Qty</th>
                        <th style='border:1px solid #ddd; padding:8px;'>Price (₹)</th>
                        <th style='border:1px solid #ddd; padding:8px;'>Subtotal (₹)</th>
                    </tr>
                </thead>
                <tbody>";

            foreach (var item in cartItems)
            {
                decimal unitPrice = item.Product.Price;
                var offer = item.Product.ProductOffer?.FirstOrDefault()?.Offer;
                if (offer != null && offer.StartDate <= DateTime.Now && offer.EndDate >= DateTime.Now)
                {
                    unitPrice -= (offer.DiscountPercentage / 100m) * unitPrice;
                }
                var subtotal = unitPrice * item.Quantity;

                receiptHtml += $@"
                    <tr>
                        <td style='border:1px solid #ddd; padding:8px;'>{item.Product.ProductName}</td>
                        <td style='border:1px solid #ddd; padding:8px;'>{item.Quantity}</td>
                        <td style='border:1px solid #ddd; padding:8px;'>{unitPrice:F2}</td>
                        <td style='border:1px solid #ddd; padding:8px;'>{subtotal:F2}</td>
                    </tr>";
            }

            receiptHtml += $@"
                </tbody>
            </table>
            <p style='margin-top:20px; font-size:16px;'><strong>Total Paid:</strong> ₹{totalOrderPrice:F2}</p>
            <p style='margin-top:20px;'>Thank you for shopping with BoxBuild! We hope you enjoy your order.</p>";

            // ✅ Send receipt via email only
            SendReceiptEmail(user.Email, "BoxBuild Payment Receipt", receiptHtml);

            // ✅ Show minimal success view
            ViewBag.Message = "Thank you! Your payment was successful. A detailed receipt has been sent to your email.";
            return View("Success");
        }


        // Razorpay DTO
        public class RazorpayOrderRequest
        {
            public int TotalAmount { get; set; } // Amount in paise
        }

        private void SendReceiptEmail(string toEmail, string subject, string bodyHtml)
        {
            var fromAddress = new MailAddress("cs2146.assco@gmail.com", "BoxBuild");
            
            var toAddress = new MailAddress(toEmail);
            const string fromPassword = "hfdhqszbboerbyir"; // Use Gmail app password

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = bodyHtml,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }

    }

}
