using Microsoft.AspNetCore.Mvc;

namespace User.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
