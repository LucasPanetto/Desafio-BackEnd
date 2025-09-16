using Microsoft.AspNetCore.Mvc;

namespace MotorcycleRental.Api.Controllers
{
    public class DeliveryManController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
