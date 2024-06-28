using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_Nikkosoft.Data;
using WMS_Nikkosoft.Models;

namespace WMS_Nikkosoft.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ApplicationDbContext _contextWMS;
        private readonly UserManager<ApplicationUser> _userManager;
        public ClientesController(AppDbContext context, UserManager<ApplicationUser> userManager, ApplicationDbContext contextWMS)
        {
            _context = context;

            _userManager = userManager;
            _contextWMS = contextWMS;

        }
        public async Task<IActionResult> Index(string sortOrder,
string currentFilter,
string searchString,
int? pageNumber)
        {


            var currentUser = await _userManager.GetUserAsync(User);


            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "cotizacion" : "";
            ViewData["DateSortParm"] = sortOrder == "cotizacion" ? "cliente" : "cotizacion";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }


            ViewData["CurrentFilter"] = searchString;
            var dataset = from s in _contextWMS.sacliente

                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                dataset = dataset.Where(s => (s.co_cli.Contains(searchString) || s.cli_des.Contains(searchString)));

            }
            if (dataset == null)
            {
                return View("NotFound");
            }
            switch (sortOrder)
            {
                case "cliente":
                    dataset = dataset.OrderByDescending(s => s.co_cli);
                    break;
                case "otizacion":
                    dataset = dataset.OrderBy(s => s.cli_des);
                    break;

                default:
                    dataset = dataset.OrderByDescending(s => s.cli_des);
                    break;
            }
            // return View(await students.AsNoTracking().ToListAsync());

            int pageSize = 8;


            // var currentUser = await _userManager.GetUserAsync(User);

            /*if (currentUser.Tipo != "Admin")
            {

                dataset = dataset.Where(c => c.vendedor == currentUser.Id);
            }*/

            return View(await PaginatedList<Cliente>.CreateAsync(dataset.AsNoTracking(), pageNumber ?? 1, pageSize));
            // return RedirectToPage()
            // return RedirectToAction("Index", "Pedido");
        }
    }
}
