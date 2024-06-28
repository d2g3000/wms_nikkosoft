using System.ComponentModel.DataAnnotations;

namespace WMS_Nikkosoft.Models
{
    public class Almacen
    {
        [Key]
        public string? co_alma { get; set; }    
        public string? des_alma { get; set; }
    }
}
