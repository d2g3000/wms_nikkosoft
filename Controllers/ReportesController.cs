using Microsoft.AspNetCore.Mvc;

namespace WMS_Nikkosoft.Controllers
{
    public class ReportesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
