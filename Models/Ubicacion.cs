using System.ComponentModel.DataAnnotations;

namespace WMS_Nikkosoft.Models
{
    public class Ubicacion
    {
        [Key]
       
        [MinLength(1),MaxLength(6)]
        [Display(Name = "Código ubicación")]
        [Required(ErrorMessage ="llene este campo")]
        public string? co_ubicacion { get; set; }
        [Display(Name = "Descripción ubicación")]
        public string? des_ubicacion { get; set; }
   
        [Range(0,10000)]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [Display(Name = "Capacidad")]
        public decimal capacidad { get; set; }
        [Display(Name = "Alto")]
        public decimal alto { get; set; }
        [Display(Name = "Largo")]
        public decimal largo { get; set; }
        [Display(Name = "Ancho")]
        public decimal ancho { get; set; }
      
        public string? co_us_in { get; set; }
       
        public DateTime? fe_us_in { get; set; }
       
        public string? campo1 { get; set; }

        public string? campo2 { get; set; }
        public string? campo3 { get; set; }
        public string? campo4 { get; set; }

        public string? campo5 { get; set; }
        public string? campo6 { get; set; }
        public string? campo7 { get; set; }

        public string? campo8 { get; set; }
        [Display(Name = "Activo")]
        public bool activo { get; set; }
        [Display(Name = "Almacén")]
        public string? co_alma { get; set; }

        // public Guid rowguid { get; set; }
    }
}
