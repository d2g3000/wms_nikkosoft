using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WMS_Nikkosoft.Data;
using WMS_Nikkosoft.Models;

namespace WMS_Nikkosoft.Controllers
{
    public class ArticulosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ApplicationDbContext _contextWMS;
        private readonly UserManager<ApplicationUser> _userManager;
        public ArticulosController(AppDbContext context, UserManager<ApplicationUser> userManager, ApplicationDbContext contextWMS)
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
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "co_art" : "";
            ViewData["DateSortParm"] = sortOrder == "co_art" ? "art_des" : "co_art";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }


            ViewData["CurrentFilter"] = searchString;
            var dataset = from s in _contextWMS.saarticulo

                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                dataset = dataset.Where(s => (s.co_art.Contains(searchString) || s.art_des.Contains(searchString)));

            }
            if (dataset == null)
            {
                return View("NotFound");
            }
            switch (sortOrder)
            {
                case "co_art":
                    dataset = dataset.OrderByDescending(s => s.co_art);
                    break;
                case "art_des":
                    dataset = dataset.OrderBy(s => s.art_des);
                    break;

                default:
                    dataset = dataset.OrderByDescending(s => s.co_art);
                    break;
            }
            // return View(await students.AsNoTracking().ToListAsync());

            int pageSize = 8;


            // var currentUser = await _userManager.GetUserAsync(User);

            /*if (currentUser.Tipo != "Admin")
            {

                dataset = dataset.Where(c => c.vendedor == currentUser.Id);
            }*/

            return View(await PaginatedList<Articulo>.CreateAsync(dataset.AsNoTracking(), pageNumber ?? 1, pageSize));
            // return RedirectToPage()
            // return RedirectToAction("Index", "Pedido");
        }
        public async Task<IActionResult> Ubicaciones(string Id)
        {
            string busqueda = Convert.ToString(Id);
            var data = await _contextWMS.nsa_saart_Ubicacion.Where(n => n.co_art == busqueda).ToListAsync();

            if (data == null)
            {


                return View("NotFound");
            }

            return View(data);
        }
    }
}
