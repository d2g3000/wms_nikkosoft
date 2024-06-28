using System.ComponentModel.DataAnnotations;

namespace WMS_Nikkosoft.Models
{
    public class Cliente
    {
        [Key]
        public string? co_cli { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Campo requerido")]
        public string? cli_des { get; set; } = "";
        [Display(Name = "Dirección")]

        public string? direc1 { get; set; } = "";
        public string? co_zon { get; set; } = "";
        public string? telefonos { get; set; } = "";
        public string? rif { get; set; } = "";
    }
}
