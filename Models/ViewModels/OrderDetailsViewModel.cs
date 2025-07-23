namespace BoxBuildproj.Models.ViewModels
{
    public class OrderDetailsViewModel
    {
        public Orders Order { get; set; }
        public List<OrderItemDetail> Items { get; set; }
    }

    public class OrderItemDetail
    {
        public string ProductName { get; set; }
        public string ProductImage { get; set; } // optional
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
