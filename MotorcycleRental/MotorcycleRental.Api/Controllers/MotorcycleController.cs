using Microsoft.AspNetCore.Mvc;

namespace MotorcycleRental.Api.Controllers
{
    public class MotorcycleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
