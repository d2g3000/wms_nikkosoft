using Microsoft.AspNetCore.Mvc;

namespace WMS_Nikkosoft.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
