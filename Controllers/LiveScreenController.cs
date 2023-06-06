using Microsoft.AspNetCore.Mvc;

namespace AgvViewWindow.Controllers
{
    public class LiveScreenController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LiveMap()
        {
            return View();
        }

        public IActionResult batteryTracking()
        {
            return View();
        }
        public IActionResult alarmTracking()
        {
            return View();
        }
    }
}
