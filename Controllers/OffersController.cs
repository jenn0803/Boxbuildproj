using Microsoft.AspNetCore.Mvc;

namespace User.Controllers
{
    public class OffersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
