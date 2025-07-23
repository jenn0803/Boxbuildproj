using System.Collections.Generic;

namespace BoxBuildproj.Models
{
    public class HomeViewModel
    {
        public List<Productstbl> AllProducts { get; set; }
        public List<TrendingProductViewModel> TrendingProducts { get; set; }
    }
}
