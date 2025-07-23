using System;
using System.Collections.Generic;

namespace BoxBuildproj.Models.ViewModels
{
    public class OrderReceiptViewModel
    {
        public int OrderId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string PaymentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal TotalPrice { get; set; }

        public List<ProductItem> Items { get; set; }

        public class ProductItem
        {
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }
    }
}
