using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WMS_Nikkosoft.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Nombre Usuario")]
        [DataType(DataType.Text)]
        public string FullName { get; set; }
        [DataType(DataType.Text)]
        public string Tipo { get; set; }
        //  [NotMapped]
        //  public List<AnalistaProducto>? analistaproducto { get; set; }
        public int cedula { get; set; }
        [Display(Name = "Rif")]
        public string? rif { get; set; }
        [Display(Name = "Código vendedor")]

        public string? direccion { get; set; }
        public string? idCliente { get; set; }
        public string? levelUser { get; set; }
    }
}
