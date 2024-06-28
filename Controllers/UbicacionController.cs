using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Data.Common;
using WMS_Nikkosoft.Data;
using WMS_Nikkosoft.Data.ViewModels;
using WMS_Nikkosoft.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WMS_Nikkosoft.Controllers
{
    [Authorize]
    public class UbicacionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ApplicationDbContext _contextWMS;
        private readonly UserManager<ApplicationUser> _userManager;
        public UbicacionController(AppDbContext context, UserManager<ApplicationUser> userManager, ApplicationDbContext contextWMS)
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
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "cotizacion" : "";
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
            var dataset = from s in _contextWMS.nsa_saUbicacion

                          select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                dataset = dataset.Where(s => (s.co_ubicacion.Contains(searchString) || s.des_ubicacion.Contains(searchString)));

            }
            if (dataset == null)
            {
                return View("NotFound");
            }
            switch (sortOrder)
            {
                case "cliente":
                    dataset = dataset.OrderByDescending(s => s.co_ubicacion);
                    break;
                case "otizacion":
                    dataset = dataset.OrderBy(s => s.des_ubicacion);
                    break;

                default:
                    dataset = dataset.OrderByDescending(s => s.des_ubicacion);
                    break;
            }
            // return View(await students.AsNoTracking().ToListAsync());

            int pageSize = 7;


            // var currentUser = await _userManager.GetUserAsync(User);

            /*if (currentUser.Tipo != "Admin")
            {

                dataset = dataset.Where(c => c.vendedor == currentUser.Id);
            }*/

            return View(await PaginatedList<Ubicacion>.CreateAsync(dataset.AsNoTracking(), pageNumber ?? 1, pageSize));
            // return RedirectToPage()
            // return RedirectToAction("Index", "Pedido");
        }
        public async Task<IActionResult> Agregar()
        {

            var lisTipoCasos = await GetComboBoxCasoVM();


            ViewBag.almacen = new SelectList(lisTipoCasos.almacen, "co_alma", "co_alma");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Agregar(Ubicacion model)
        {
            var lisTipoCasos = await GetComboBoxCasoVM();


            if (!ModelState.IsValid)
            {

                ViewBag.almacen = new SelectList(lisTipoCasos.almacen, "co_alma", "co_alma");
                return View(model);
            }
            var codigo = await _contextWMS.nsa_saUbicacion.FirstOrDefaultAsync(x=>x.co_ubicacion== model.co_ubicacion);
            if (codigo != null)
            {

                ViewBag.almacen = new SelectList(lisTipoCasos.almacen, "co_alma", "co_alma");
                TempData["Error"] = "Este código de ubicacion esta en uso";
                return View(model);
            }

            var descripcion = await _contextWMS.nsa_saUbicacion.FirstOrDefaultAsync(x => x.des_ubicacion == model.des_ubicacion);
            if (descripcion != null)
            {

                ViewBag.almacen = new SelectList(lisTipoCasos.almacen, "co_alma", "co_alma");
                TempData["Error"] = "Esta descripción de ubicacion esta en uso";
                return View(model);
            }

            var entry = new Ubicacion
            {

                co_ubicacion = model.co_ubicacion,
                des_ubicacion = model.des_ubicacion,
                capacidad = model.capacidad,
                alto = model.alto,
                largo = model.largo,
                ancho = model.ancho,
                co_us_in = "profit",
                fe_us_in = DateTime.Now,
                campo1 = model.campo1,
                campo2 = model.campo2,
                campo3 = model.campo3,
                campo4 = model.campo4,
                campo5 = model.campo5,
                campo6 = model.campo6,
                campo7 = model.campo7,
                campo8 = model.campo8,
                activo = model.activo,
                co_alma = model.co_alma,
              //  rowguid = new Guid()

            };
        
            await _contextWMS.AddAsync(entry);
            await _contextWMS.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        /* public async Task<IActionResult> Eliminar()
         {
             return View();
         }*/


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

      
            var valor = await _contextWMS.nsa_saart_Ubicacion.FirstOrDefaultAsync(n => n.ubicacion == id);
            if (valor != null)
            {
                
                return Json(new { status = "EnUso" });
            }

            var entity = await _contextWMS.nsa_saUbicacion.FirstOrDefaultAsync(n => n.co_ubicacion == id);
            EntityEntry entityEntry = _contextWMS.Entry<Ubicacion>(entity);
            entityEntry.State = EntityState.Deleted;
            await _contextWMS.SaveChangesAsync();
        
            return Json(new { status = "Borrado" });
        }

        public async Task<IActionResult> Editar(string Id)
        {
            string busqueda = Convert.ToString(Id);
            var data = await _contextWMS.nsa_saUbicacion.FirstOrDefaultAsync(n => n.co_ubicacion == busqueda);
         
            if (data == null)
            {

              
                return View("NotFound");
            }

            var lisTipoCasos = await GetComboBoxCasoVM();


            ViewBag.almacen = new SelectList(lisTipoCasos.almacen, "co_alma", "co_alma");
            return View(data);
  
        }
        [HttpPost]
        public async Task<IActionResult> Editar(Ubicacion ubicacion)
        {
            var data = await _contextWMS.nsa_saUbicacion.FirstOrDefaultAsync(n => n.co_ubicacion == ubicacion.co_ubicacion);

            if (data != null)
            {
                data.des_ubicacion = ubicacion.des_ubicacion;
                data.capacidad = ubicacion.capacidad;
                data.alto = ubicacion.alto;
                data.largo = ubicacion.largo;
                data.ancho = ubicacion.ancho;
                data.co_alma = ubicacion.co_alma;

                _contextWMS.Update(data);
                await _contextWMS.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Editar), "Ubicacion", new { Id = data.co_ubicacion });
        }
        public async Task<ComboBoxCasoVM> GetComboBoxCasoVM()
        {
            var response = new ComboBoxCasoVM()
            {
                almacen = await _contextWMS.saalmacen.Where(x => x.co_alma != null).OrderBy(n => n.co_alma).ToListAsync(),
             

            };

            return response;
        }


        [HttpGet]
        [Authorize]
        [Route("listadoUbicaciones/{ubicacion}")]
        public JsonResult listadoUbicaciones(string ubicacion)
        {
            List<ListadoUbicacionesVM> lista = new List<ListadoUbicacionesVM>();

            var conn = _context.Database.GetDbConnection();
            conn.Open();
            using (var command = conn.CreateCommand())
            {
                string query = " select top(10) co_ubicacion,des_ubicacion from nsa_saUbicacion "+
                    " where co_ubicacion like '%"+ubicacion+"%' or des_ubicacion like '%"+ubicacion+"%' ";
                command.CommandText = query;
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        lista.Add(new ListadoUbicacionesVM()
                        {
                            codigo = Convert.ToString(reader["co_ubicacion"]),
                            descripcion = Convert.ToString(reader["des_ubicacion"]),
                            


                        }); ;

                    }
                }
                reader.Dispose();
            }


            return Json(new { data = lista });
        }
    }
}
