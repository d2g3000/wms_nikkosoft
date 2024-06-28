using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WMS_Nikkosoft.Models;

namespace WMS_Nikkosoft.Data
{
    public class ApplicationDbContext  :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Cliente>? sacliente { get; set; }
        public DbSet<Proveedor>? saproveedor { get; set; }

        public DbSet<Recepcion>? saFacturaCompra { get; set; }

        public DbSet<Entrega>? saNotaEntregaVenta { get; set; }

        public DbSet<Ubicacion>? nsa_saUbicacion { get; set; }

        public DbSet<Articulo>? saarticulo { get; set; }

        public DbSet<RecepcionReng>? saFacturaCompraReng { get; set; }
        public DbSet<EntregaReng>? saNotaEntregaVentaReng { get; set; }
        public DbSet<SaArtUbicacion>? nsa_saart_Ubicacion { get; set; }
        public DbSet<SaArtUbicacionSalida>? nsa_saart_UbicacionSalida { get; set; }
        public DbSet<Almacen>? saalmacen { get; set; }
    }
}
