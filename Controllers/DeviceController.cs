using Microsoft.AspNetCore.Mvc;

namespace GameZone.Controllers
{
    public class DeviceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
