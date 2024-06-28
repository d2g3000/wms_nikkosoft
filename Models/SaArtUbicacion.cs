using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_Nikkosoft.Models
{
    [Keyless]
    public class SaArtUbicacion
    {
        public Guid rowguid_reng { get; set; }
        public string? co_art { get; set; }
        [ForeignKey("co_art")]
        public Articulo? articulo { get; set; }
        public string? co_alma { get; set; }
        public int reng_num { get; set; }
        public decimal cantidad { get; set; }

        public string? ubicacion { get; set; }
        public string? tipo_doc { get; set; }
        public decimal stock_actual { get; set; }
        public Guid rowguid { get; set; }
    }
}
