using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Net.NetworkInformation;
using WMS_Nikkosoft.Data;
using WMS_Nikkosoft.Data.ViewModels;
using WMS_Nikkosoft.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WMS_Nikkosoft.Controllers
{
    [Authorize]
    public class RecepcionesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ApplicationDbContext _contextWMS;
        private readonly UserManager<ApplicationUser> _userManager;
        public RecepcionesController(AppDbContext context, UserManager<ApplicationUser> userManager, ApplicationDbContext contextWMS)
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
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "doc_num" : "";
            ViewData["DateSortParm"] = sortOrder == "doc_num" ? "descrip" : "doc_num";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }


            ViewData["CurrentFilter"] = searchString;
            var dataset = from s in _contextWMS.saFacturaCompra

                          select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                dataset = dataset.Where(s => (s.doc_num.Contains(searchString) || s.nro_fact.Contains(searchString)));

            }
            if (dataset == null)
            {
                return View("NotFound");
            }
            switch (sortOrder)
            {
                case "doc_num":
                    dataset = dataset.OrderByDescending(s => s.doc_num);
                    break;
                case "descrip":
                    dataset = dataset.OrderBy(s => s.descrip);
                    break;

                default:
                    dataset = dataset.OrderByDescending(s => s.doc_num);
                    break;
            }
            // return View(await students.AsNoTracking().ToListAsync());

            int pageSize = 8;


            // var currentUser = await _userManager.GetUserAsync(User);

            /*if (currentUser.Tipo != "Admin")
            {

                dataset = dataset.Where(c => c.vendedor == currentUser.Id);
            }*/

            return View(await PaginatedList<Recepcion>.CreateAsync(dataset.AsNoTracking(), pageNumber ?? 1, pageSize));
            // return RedirectToPage()
            // return RedirectToAction("Index", "Pedido");
        }
        [Route("Consultar")]
        public async Task<IActionResult> Consultar(string Id)
        {



            List<RecepcionVM> lista = new List<RecepcionVM>();

            var conn = _context.Database.GetDbConnection();
            conn.Open();
            using (var command = conn.CreateCommand())
            {
                string query = " select T1.reng_num,T1.co_art,T1.doc_num,T1.rowguid,T1.total_art,  +" +
                "  CASE " +
                " WHEN T1.rowguid in ( select rowguid_reng from nsa_saart_Ubicacion " +
                " where rowguid_reng = T1.rowguid) THEN ((select isnull(SUM(cantidad),0) " +
                " from nsa_saart_Ubicacion where rowguid_reng = T1.rowguid)) " +
                " WHEN T1.rowguid not in ( select rowguid_reng from nsa_saart_Ubicacion " +
                " where rowguid_reng = T1.rowguid) THEN ((select isnull(SUM(cantidad),0) " +
                " from nsa_saart_Ubicacion where rowguid_reng = T1.rowguid)) " +
                " END as cant_asignada, " +
                " CASE " +
                " WHEN T1.rowguid in ( select rowguid_reng from nsa_saart_Ubicacion " +
                " where rowguid_reng = T1.rowguid) THEN 1 " +
                " WHEN T1.rowguid not in ( select rowguid_reng from nsa_saart_Ubicacion " +
                " where rowguid_reng = T1.rowguid) THEN 0 " +
                " END as existe," +
                " CASE " +
                " WHEN T1.total_art = (select isnull(SUM(cantidad),0) from nsa_saart_Ubicacion " +
                " where rowguid_reng = T1.rowguid) THEN 'TOTALMENTE ASIGNADO' " +
                " WHEN T1.total_art > (select isnull(SUM(cantidad),0) from nsa_saart_Ubicacion " +
                " where rowguid_reng = T1.rowguid) AND (select isnull(SUM(cantidad),0) " +
                " from nsa_saart_Ubicacion where rowguid_reng = T1.rowguid) > 0 THEN 'MEDIO ASIGNADO' " +
                " WHEN T1.total_art > (select isnull(SUM(cantidad),0) " +
                " from nsa_saart_Ubicacion where rowguid_reng = T1.rowguid) " +
                " AND (select isnull(SUM(cantidad),0) from nsa_saart_Ubicacion " +
                " where rowguid_reng = T1.rowguid) <= 0THEN 'NO ASIGNADO' " +
                " END as estatus " +
                " from saFacturaCompraReng T1 where T1.doc_num = '" + Id.Trim() + "' " +
                " ORDER BY T1.reng_num ";

                command.CommandText = query;
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        lista.Add(new RecepcionVM()
                        {
                            reng_num = Convert.ToInt32(reader["reng_num"]),
                            co_art = Convert.ToString(reader["co_art"]),
                            doc_num = Convert.ToString(reader["doc_num"]),
                            rowguid = Convert.ToString(reader["rowguid"]),
                            total_art = Convert.ToDecimal(reader["total_art"]),
                            cant_asignada = Convert.ToDecimal(reader["cant_asignada"]),
                            existe = Convert.ToInt32(reader["existe"]),
                            estatus = Convert.ToString(reader["estatus"])

                        });

                    }
                }
                reader.Dispose();


                // reng_num,co_art,doc_num,rowguid,total_art,cant_asignada,existe,estatus
            }
            ViewData["doc_num"] = Id;
            var rowDoc = _contextWMS.saFacturaCompra.FirstOrDefault(x => x.doc_num == Id);

            ViewData["doc_rowguid"] = rowDoc.rowguid;

            return View(lista);
            /* string StoredProc = " select T1.reng_num,T1.co_art,T1.doc_num,T1.rowguid,T1.total_art,  +" +
                 "  CASE "+
                 " WHEN T1.rowguid in ( select rowguid_reng from nsa_saart_Ubicacion "+
                 " where rowguid_reng = T1.rowguid) THEN ((select isnull(SUM(cantidad),0) "+
                 " from nsa_saart_Ubicacion where rowguid_reng = T1.rowguid)) "+
                 " WHEN T1.rowguid not in ( select rowguid_reng from nsa_saart_Ubicacion "+
                 " where rowguid_reng = T1.rowguid) THEN ((select isnull(SUM(cantidad),0) "+
                 " from nsa_saart_Ubicacion where rowguid_reng = T1.rowguid)) "+
                 " END as Cant_Asignada, "+
                 " CASE "+
                 " WHEN T1.rowguid in ( select rowguid_reng from nsa_saart_Ubicacion "+
                 " where rowguid_reng = T1.rowguid) THEN 1 "+
                 " WHEN T1.rowguid not in ( select rowguid_reng from nsa_saart_Ubicacion "+
                 " where rowguid_reng = T1.rowguid) THEN 0 "+
                 " END as Existe,"+
                 " CASE "+
                 " WHEN T1.total_art = (select isnull(SUM(cantidad),0) from nsa_saart_Ubicacion "+
                 " where rowguid_reng = T1.rowguid) THEN 'TOTALMENTE ASIGNADO' "+
                 " WHEN T1.total_art > (select isnull(SUM(cantidad),0) from nsa_saart_Ubicacion "+
                 " where rowguid_reng = T1.rowguid) AND (select isnull(SUM(cantidad),0) "+
                 " from nsa_saart_Ubicacion where rowguid_reng = T1.rowguid) > 0 THEN 'MEDIO ASIGNADO' "+
                 " WHEN T1.total_art > (select isnull(SUM(cantidad),0) "+
                 " from nsa_saart_Ubicacion where rowguid_reng = T1.rowguid) "+
                 " AND (select isnull(SUM(cantidad),0) from nsa_saart_Ubicacion "+
                 " where rowguid_reng = T1.rowguid) <= 0THEN 'NO ASIGNADO' "+
                 " END as Estatus "+
                 " from saFacturaCompraReng T1 where T1.doc_num = '"+Id.Trim()+"' "+
                 " ORDER BY T1.reng_num ";



             await _context.Database.ExecuteSqlRawAsync(StoredProc);
            return RedirectToAction(nameof(Index));
             */


            /* var data = await _contextWMS.saFacturaCompraReng
                 .Where(n => n.doc_num == Id).OrderBy(o=>o.reng_num).ToListAsync();

             if (data == null)
             {
                 return View("NotFound");
             }
             return View(data);*/
        }

        [Route("UbicarFacturaCompra")]
        [HttpPost]
        public async Task<IActionResult> UbicarFacturaCompra([FromBody]RenglonesFacturaCompraVM fcreng)
        {
            foreach (var item in fcreng.RenglonesCompra)
            {
                var connR =  _contextWMS.Database.GetDbConnection();
                connR.Open();
                using (var commandR = connR.CreateCommand())
                {

                    string StoredProc = " exec [nsa_saInsertarubicacion_facturacompra] '" + item.rowguid + "'  ";

                    await _contextWMS.Database.ExecuteSqlRawAsync(StoredProc);

                    commandR.CommandText = StoredProc;
                    DbDataReader readerR = await commandR.ExecuteReaderAsync();

                    if (readerR.HasRows)
                    {
                         while (readerR.Read())
                         {
                            //onsecutivo = Convert.ToString(readerH["ProximoConsecutivo"]);

                         }
                    }
                     readerR.Dispose();

                }
                connR.Close();

            }
            return Json(new { data = "logrado", Id = fcreng.docnum });
          //  return  RedirectToAction(nameof(Consultar), "Recepciones", new { Id = fcreng.docnum });
        }
        public async Task<IActionResult> Ubicar(string Id)
        {
            
            var data = _contextWMS.saFacturaCompraReng.Include(c=>c.articulo).FirstOrDefault(x=>x.rowguid.ToString()==Id.ToString());
            if (data==null)
            {
                return View("NotFound"); 
            }
            return View(data);
        }

        [HttpGet]
        [Authorize]
        [Route("UbicacionRenglones/{Id}")]
        public  JsonResult UbicacionRenglones(string Id)
        {
            List<ArticuloUbicacionVM> lista = new List<ArticuloUbicacionVM>();
            string busqueda = Convert.ToString(Id);
            var data = _contextWMS.nsa_saart_Ubicacion.Where(n => n.rowguid_reng.ToString() == busqueda).ToListAsync();
            foreach (var item in data.Result)
            {

                lista.Add(new ArticuloUbicacionVM()
                {
                    codigo = item.co_art,
                    cantidad = item.cantidad,
                    ubicacion = item.ubicacion
                });
            }

            return Json(new { data=lista});
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> InsertarRenglonUbicacion(InsertRengUbiVM vm)
        {


            var connR = _contextWMS.Database.GetDbConnection();
            connR.Open();
            using (var commandR = connR.CreateCommand())
            {
                string StoredProc = "exec dbo.nsa_pInsertarRenglonesUbicacion " +
                                " @gRowguid_Reng ='"+vm.gRowguid_Reng+"' , " +
                                " @iReng_Num = "+vm.iReng_Num+", " +
                                " @sTipo_Doc = '"+vm.sTipo_Doc+"', " +
                                " @sCo_Art = '"+vm.sCo_Art+"' , " +
                                " @sCo_Alma = '"+vm.sCo_Alma+"' , " +
                                " @sUbicacion = '"+vm.sUbicacion+"' , " +
                                " @sdFecha_Inicio = '"+vm.sdFecha_Inicio+"', " +
                                " @sdFecha_Expiracion = NULL , " +
                                " @deCantidad = "+vm.deCantidad+" , " +
                                " @co_procedencia = NULL, " +
                                " @dePrecio = "+vm.dePrecio+", " +
                                " @sCo_Us_In = '"+vm.sCo_Us_In+"' , " +
                                " @sMaquina = '"+vm.sMaquina+"' ," +
                                " @sCo_Sucu_In = '"+vm.sCo_Sucu_In+"' , " +
                                " @sRevisado = '"+vm.sRevisado+"', " +
                                " @sTrasnfe = '"+vm.sTrasnfe+"' ";

                /* @IdCaso =" + caso.IdCaso + ", " +
                 " @UsuarioAplicacion='" + user_App + "',@UsuarioWindows='" + user_App + "', " +
                 " @EquipoUsuario='" + user_App + "' ";*/
          
                await _contextWMS.Database.ExecuteSqlRawAsync(StoredProc);

              //  commandR.CommandText = StoredProc;
               // DbDataReader readerR = await commandR.ExecuteReaderAsync();

               // if (readerR.HasRows)
               // {
                 //   while (readerR.Read())
                //    {
                        //onsecutivo = Convert.ToString(readerH["ProximoConsecutivo"]);

                   // }
               // }
               // readerR.Dispose();

            }
            connR.Close();

        
            

            //return await _context.output.ToListAsync();
            // await _contextWMS.Database.ExecuteSqlRawAsync(StoredProc);

            return Json(new { status = "EnUso" });

        }
    }

}
//[nsa_saInsertarubicacion_facturacompra] @rowguid

/*
 * [dbo].[nsa_pInsertarRenglonesUbicacion]
    (
      @gRowguid_Reng UNIQUEIDENTIFIER ,
      @iReng_Num INT ,
      @sTipo_Doc CHAR(4) ,
      @sCo_Art CHAR(30) = NULL ,
      @sCo_Alma CHAR(6) = NULL ,
      @sUbicacion CHAR(20) = NULL ,
      @sdFecha_Inicio SMALLDATETIME = NULL ,
      @sdFecha_Expiracion SMALLDATETIME = NULL ,
      @deCantidad DECIMAL(18, 5) ,
	  @co_procedencia VARCHAR(6),
      @dePrecio DECIMAL(18, 5) ,
      @sCo_Us_In CHAR(6) ,
      @sMaquina VARCHAR(60) = NULL ,
      @sCo_Sucu_In CHAR(6) = NULL ,
      @sRevisado CHAR(1) = NULL ,
      @sTrasnfe CHAR(1) = NULL
    )

*/
