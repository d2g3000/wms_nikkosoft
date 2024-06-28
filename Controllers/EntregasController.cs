using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_Nikkosoft.Data;
using WMS_Nikkosoft.Models;

namespace WMS_Nikkosoft.Controllers
{
    [Authorize]
    public class EntregasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ApplicationDbContext _contextWMS;
        private readonly UserManager<ApplicationUser> _userManager;
        public EntregasController(AppDbContext context, UserManager<ApplicationUser> userManager, ApplicationDbContext contextWMS)
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
            var dataset = from s in _contextWMS.saNotaEntregaVenta

                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                dataset = dataset.Where(s => (s.doc_num.Contains(searchString) || s.co_cli.Contains(searchString)));

            }
            if (dataset == null)
            {
                return View("NotFound");
            }
            switch (sortOrder)
            {
                case "cliente":
                    dataset = dataset.OrderByDescending(s => s.doc_num);
                    break;
                case "otizacion":
                    dataset = dataset.OrderBy(s => s.co_cli);
                    break;

                default:
                    dataset = dataset.OrderByDescending(s => s.descrip);
                    break;
            }
            // return View(await students.AsNoTracking().ToListAsync());

            int pageSize = 8;


            // var currentUser = await _userManager.GetUserAsync(User);

            /*if (currentUser.Tipo != "Admin")
            {

                dataset = dataset.Where(c => c.vendedor == currentUser.Id);
            }*/

            return View(await PaginatedList<Entrega>.CreateAsync(dataset.AsNoTracking(), pageNumber ?? 1, pageSize));
            // return RedirectToPage()
            // return RedirectToAction("Index", "Pedido");
        }
        public async Task<IActionResult> Consultar(string Id)
        {
            var data = await _contextWMS.saNotaEntregaVentaReng
                .Where(n => n.doc_num == Id).OrderBy(o => o.reng_num).ToListAsync();

            if (data == null)
            {
                return View("NotFound");
            }
            return View(data);
        }
    }
}
