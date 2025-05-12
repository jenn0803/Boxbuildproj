namespace BoxBuildproj.Models
{
    public class CheckoutViewModel
    {
        public string RazorpayKey { get; set; }
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "INR";
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }

        // Optional for UI
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
    }

}
