using System;
using System.Collections.Generic;

namespace BoxBuildproj.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; }

        public decimal TotalAmount { get; set; }

        public List<OrderItemViewModel> OrderItems { get; set; } = new();
    }

    public class OrderItemViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string? ImagePath { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal SubTotal => Price * Quantity;
    }
}
