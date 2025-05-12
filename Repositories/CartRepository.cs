//using BoxBuildproj.Models;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.EntityFrameworkCore;
//using System;
//using BoxBuildproj.Data;

//namespace BoxBuildproj.Repositories
//{
//    public class CartRepository
//    {
//        private readonly BoxBuildprojContext _context;

//        public CartRepository(BoxBuildprojContext context)
//        {
//            _context = context;
//        }

//        public List<CartItem> GetCartItems(int userId)
//        {
//            return _context.Carts
//                .Where(c => c.UserId == userId)
//                .Include(c => c.Product)
//                .Select(c => new CartItem
//                {
//                    CartId = c.CartId,
//                    UserId = c.UserId,
//                    Product = c.Product,
//                    Quantity = c.Quantity
//                }).ToList();
//        }

//        public decimal GetTotalAmount(int userId)
//        {
//            return GetCartItems(userId).Sum(item => item.TotalPrice);
//        }

//        public void ClearCart(int userId)
//        {
//            var cartItems = _context.Carts.Where(c => c.UserId == userId);
//            _context.Carts.RemoveRange(cartItems);
//            _context.SaveChanges();
//        }
//    }
//}
