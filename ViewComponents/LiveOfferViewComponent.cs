using BoxBuildproj.Data; // Your DbContext namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BoxBuildproj.ViewComponents
{
    public class LiveOfferViewComponent : ViewComponent
    {
        private readonly BoxBuildprojContext _context;

        public LiveOfferViewComponent(BoxBuildprojContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var today = DateTime.Today;

            var offer = await _context.Offer
                .Where(o => o.StartDate <= today && o.EndDate >= today)
                .OrderBy(o => o.StartDate)
                .FirstOrDefaultAsync();

            return View(offer);
        }
    }
}
