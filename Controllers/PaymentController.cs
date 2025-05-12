//using Microsoft.AspNetCore.Mvc;
//using Stripe;

//namespace User.Controllers
//{
//    public class PaymentController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//        public ActionResult Pay()
//        {
//            var options = new ChargeCreateOptions
//            {
//                Amount = 5000, // ₹50.00
//                Currency = "inr",
//                Description = "Order Payment",
//                Source = "tok_visa",
//            };
//            var service = new ChargeService();
//            Charge charge = service.Create(options);
//            return View();
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace BoxBuildproj.Controllers
{
    public class PaymentController : Controller
    {
        private readonly string key;
        private readonly string secret;

        public PaymentController(IConfiguration configuration)
        {
            key = configuration["Razorpay:Key"];
            secret = configuration["Razorpay:Secret"];
        }

        // Checkout Page
        public IActionResult Checkout(int userId, decimal totalAmount)
        {
            int amountInPaise = (int)(totalAmount * 100);

            RazorpayClient client = new RazorpayClient(key, secret);

            Dictionary<string, object> options = new Dictionary<string, object>
            {
                { "amount", amountInPaise },
                { "currency", "INR" },
                { "payment_capture", 1 }
            };

            Order order = client.Order.Create(options);

            ViewBag.OrderId = order["id"].ToString();
            ViewBag.RazorpayKey = key;
            ViewBag.Amount = amountInPaise;
            ViewBag.UserId = userId;

            return View();
        }

        // Callback After Payment
        [HttpPost]
        public IActionResult PaymentSuccess(string razorpay_payment_id, string razorpay_order_id, string razorpay_signature)
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>
            {
                { "razorpay_payment_id", razorpay_payment_id },
                { "razorpay_order_id", razorpay_order_id },
                { "razorpay_signature", razorpay_signature }
            };

            try
            {
                Utils.verifyPaymentSignature(attributes);
                // Store payment info to DB if needed
                return View("Success");
            }
            catch
            {
                return View("Failure");
            }
        }
    }
}
