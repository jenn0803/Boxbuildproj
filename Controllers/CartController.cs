//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using BoxBuildproj.Data;
//using BoxBuildproj.Models;
//using Microsoft.AspNetCore.Identity;
//using System.Linq;
//using System.Threading.Tasks;
//using BoxBuildproj.Areas.Identity.Data;

//namespace BoxBuildproj.Controllers
//{
//    public class CartController : Controller
//    {
//        private readonly BoxBuildprojContext _context;
//        private readonly UserManager<BoxBuildprojUser> _userManager;

//        public CartController(BoxBuildprojContext context, UserManager<BoxBuildprojUser> userManager)
//        {
//            _context = context;
//            _userManager = userManager;
//        }

//        public async Task<IActionResult> Index()
//        {
//            var user = await _userManager.GetUserAsync(User);
//            if (user == null) return RedirectToAction("Login", "Account");

//            var cartItems = await _context.Carts
//                                .Include(c => c.Product)
//                                .Where(c => c.UserId == user.Id)
//                                .ToListAsync();

//            return View(cartItems);
//        }

//        public async Task<IActionResult> AddToCart(int id)
//        {
//            var user = await _userManager.GetUserAsync(User);
//            if (user == null) return RedirectToAction("Login", "Account");

//            var cartItem = new Cart
//            {
//                UserId = user.Id,
//                ProductId = id,
//                Quantity = 1
//            };

//            _context.Carts.Add(cartItem);
//            await _context.SaveChangesAsync();
//            return RedirectToAction("Index", "Product");
//        }

//        public async Task<IActionResult> AddToWishlist(int id)
//        {
//            var user = await _userManager.GetUserAsync(User);
//            if (user == null) return RedirectToAction("Login", "Account");

//            var wishlistItem = new Wishlist
//            {
//                UserId = user.Id,
//                ProductId = id
//            };

//            _context.Wishlists.Add(wishlistItem);
//            await _context.SaveChangesAsync();
//            return RedirectToAction("Index", "Product");
//        }
//    }
//}


//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using BoxBuildproj.Data;
//using BoxBuildproj.Models;

//namespace BoxBuildproj.Controllers
//{
//    [Authorize] // 👈 Ensure user is logged in
//    public class CartController : Controller
//    {
//        private readonly BoxBuildprojContext _context;

//        public CartController(BoxBuildprojContext context)
//        {
//            _context = context;
//        }

//        public async Task<IActionResult> Index()
//        {
//            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//            var cartItems = await _context.Carts
//                .Where(c => c.UserId == userId)
//                .Include(c => c.Product)
//                .ToListAsync();

//            return View(cartItems);
//        }

//        public async Task<IActionResult> AddToCart(int productId)
//        {
//            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//            var existingCartItem = await _context.Carts
//                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

//            if (existingCartItem != null)
//            {
//                existingCartItem.Quantity++;
//            }
//            else
//            {
//                var cartItem = new Cart
//                {
//                    UserId = userId,
//                    ProductId = productId,
//                    Quantity = 1
//                };
//                _context.Carts.Add(cartItem);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction("Index");
//        }

//        public async Task<IActionResult> RemoveFromCart(int id)
//        {
//            var cartItem = await _context.Carts.FindAsync(id);
//            if (cartItem != null)
//            {
//                _context.Carts.Remove(cartItem);
//                await _context.SaveChangesAsync();
//            }
//            return RedirectToAction("Index");
//        }
//    }
//}


//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Identity;
//using System.Linq;
//using System.Threading.Tasks;
//using BoxBuildproj.Data;
//using BoxBuildproj.Models;
//using System.Security.Claims;
//using BoxBuildproj.Areas.Identity.Data;

//public class CartController : Controller
//{
//    private readonly BoxBuildprojContext _context;
//    private readonly UserManager<BoxBuildprojUser> _userManager;

//    public CartController(BoxBuildprojContext context, UserManager<BoxBuildprojUser> userManager)
//    {
//        _context = context;
//        _userManager = userManager;
//    }

//    // ✅ Display Cart Items
//    public async Task<IActionResult> Index()
//    {
//        var user = await _userManager.GetUserAsync(User);
//        if (user == null)
//        {
//            return RedirectToAction("Login", "Account");
//        }

//        var cartItems = _context.Carts
//            .Where(c => c.UserId == user.Id)
//            .ToList();

//        return View(cartItems);
//    }

//    // ✅ Update Cart Quantity
//    [HttpPost]
//    public async Task<IActionResult> UpdateQuantity(int cartId, int quantity)
//    {
//        var cartItem = _context.Carts.FirstOrDefault(c => c.CartId == cartId);
//        if (cartItem != null)
//        {
//            cartItem.Quantity = quantity;
//            _context.Carts.Update(cartItem);
//            await _context.SaveChangesAsync();
//        }
//        return RedirectToAction("Index");
//    }

//    // ✅ Add to Cart
//    [HttpPost]
//    public async Task<IActionResult> AddToCart(int productId, int quantity)
//    {
//        var user = await _userManager.GetUserAsync(User);
//        if (user == null)
//        {
//            return RedirectToAction("Login", "Account"); // Redirect if user is not logged in
//        }

//        var cartItem = _context.Carts.FirstOrDefault(c => c.UserId == user.Id && c.ProductId == productId);

//        if (cartItem == null)
//        {
//            // If item is not in the cart, add new
//            var newCartItem = new Cart
//            {
//                UserId = user.Id,
//                ProductId = productId,
//                Quantity = quantity
//            };
//            _context.Carts.Add(newCartItem);
//        }
//        else
//        {
//            // If item exists, update the quantity
//            cartItem.Quantity = quantity;
//            _context.Carts.Update(cartItem);
//        }

//        await _context.SaveChangesAsync();
//        return RedirectToAction("Index");
//    }

//    // ✅ Remove Item from Cart
//    [HttpPost]
//    public async Task<IActionResult> RemoveFromCart(int cartId)
//    {
//        var cartItem = _context.Carts.FirstOrDefault(c => c.CartId == cartId);
//        if (cartItem != null)
//        {
//            _context.Carts.Remove(cartItem);
//            await _context.SaveChangesAsync();
//        }
//        return RedirectToAction("Index");
//    }
//}


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using BoxBuildproj.Data;
using BoxBuildproj.Models;
using BoxBuildproj.Areas.Identity.Data;
using Razorpay.Api;
using Microsoft.AspNetCore.Authorization;


[Authorize]
public class CartController : Controller
{
    private readonly BoxBuildprojContext _context;
    private readonly UserManager<BoxBuildprojUser> _userManager;

    public CartController(BoxBuildprojContext context, UserManager<BoxBuildprojUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // ✅ Display Cart Items with Product Details
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

        ViewBag.Subtotal = cartItems.Sum(item =>
        {
            var price = item.Product.Price;
            var offer = item.Product.ProductOffer?.FirstOrDefault()?.Offer;
            if (offer != null && offer.StartDate <= DateTime.Now && offer.EndDate >= DateTime.Now)
            {
                var discount = (offer.DiscountPercentage / 100.0m) * price;
                price -= discount;
            }
            return price * item.Quantity;
        });


        return View(cartItems);
    }



    // ✅ Update Cart Quantity (Ensure min value is 1)
    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int cartId, int quantity)
    {
        if (quantity < 1) quantity = 1; // Prevent invalid quantity

        var cartItem = await _context.Carts.FindAsync(cartId);
        if (cartItem != null)
        {
            cartItem.Quantity = quantity;
            _context.Carts.Update(cartItem);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

    // ✅ Add to Cart (Increment if already exists)
    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var cartItem = await _context.Carts
            .FirstOrDefaultAsync(c => c.UserId == user.Id && c.ProductId == productId);

        if (cartItem == null)
        {
            _context.Carts.Add(new Cart
            {
                UserId = user.Id,
                ProductId = productId,
                Quantity = quantity
            });
        }
        else
        {
            cartItem.Quantity += quantity; // Increment quantity
            _context.Carts.Update(cartItem);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }


    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var cartItems = await _context.Carts
            .Include(c => c.Product)
            .Where(c => c.UserId == user.Id)
            .ToListAsync();

        if (!cartItems.Any()) return RedirectToAction("Index");

        decimal totalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity);
        int amountInPaise = (int)(totalAmount * 100);

        RazorpayClient client = new RazorpayClient("YOUR_KEY_ID", "YOUR_KEY_SECRET");

        var options = new Dictionary<string, object>
    {
        { "amount", amountInPaise },
        { "currency", "INR" },
        { "receipt", Guid.NewGuid().ToString() },
        { "payment_capture", 1 }
    };

        Order order = client.Order.Create(options);

        var model = new CheckoutViewModel
        {
            RazorpayKey = "YOUR_KEY_ID",
            OrderId = order["id"].ToString(),
            Amount = totalAmount,
            Name = user.UserName,
            Email = user.Email,
            Contact = "9999999999"
        };

        return View(model);
    }


    // ✅ Remove Item from Cart
    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int cartId)
    {
        var cartItem = await _context.Carts.FindAsync(cartId);
        if (cartItem != null)
        {
            _context.Carts.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
}
