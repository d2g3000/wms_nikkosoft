using System.ComponentModel.DataAnnotations;

namespace WMS_Nikkosoft.Models
{
    public class Articulo
    {
        [Key]
        public string? co_art { get; set; }
        public string?  art_des { get; set; }
        public string? co_lin { get; set; }
        public string? co_subl { get; set; }
        public string? co_cat { get; set; }
        public string? modelo { get; set; }
        /* public string volumen { get; set; }
         public string peso { get; set; }*/
    }
}
