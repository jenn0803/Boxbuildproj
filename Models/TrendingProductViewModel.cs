namespace BoxBuildproj.Models
{
    public class TrendingProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public int TotalSold { get; set; }
    }
}
