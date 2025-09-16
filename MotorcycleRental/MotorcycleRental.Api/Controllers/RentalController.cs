using Microsoft.AspNetCore.Mvc;

namespace MotorcycleRental.Api.Controllers
{
    public class RentalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
