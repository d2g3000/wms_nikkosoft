using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_Nikkosoft.Data;
using WMS_Nikkosoft.Models;

namespace WMS_Nikkosoft.Controllers
{
    [Authorize]
    public class ProveedoresController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ApplicationDbContext _contextWMS;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProveedoresController(AppDbContext context, UserManager<ApplicationUser> userManager, ApplicationDbContext contextWMS)
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
            var dataset = from s in _contextWMS.saproveedor

                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                dataset = dataset.Where(s => (s.co_prov.Contains(searchString) || s.prov_Des.Contains(searchString)));

            }
            if (dataset == null)
            {
                return View("NotFound");
            }
            switch (sortOrder)
            {
                case "cliente":
                    dataset = dataset.OrderByDescending(s => s.co_prov);
                    break;
                case "otizacion":
                    dataset = dataset.OrderBy(s => s.prov_Des);
                    break;

                default:
                    dataset = dataset.OrderByDescending(s => s.prov_Des);
                    break;
            }
            // return View(await students.AsNoTracking().ToListAsync());

            int pageSize = 8;


            // var currentUser = await _userManager.GetUserAsync(User);

            /*if (currentUser.Tipo != "Admin")
            {

                dataset = dataset.Where(c => c.vendedor == currentUser.Id);
            }*/

            return View(await PaginatedList<Proveedor>.CreateAsync(dataset.AsNoTracking(), pageNumber ?? 1, pageSize));
            // return RedirectToPage()
            // return RedirectToAction("Index", "Pedido");
        }
    }
}
