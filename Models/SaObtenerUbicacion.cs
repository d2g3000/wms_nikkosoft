using Microsoft.EntityFrameworkCore;

namespace WMS_Nikkosoft.Models
{
    [Keyless]
    public class SaObtenerUbicacion
    {
        public string? lote { get; set; }
        public decimal stock { get; set; }

        public DateTime fecha_expiracion { get; set; }
        public Guid rowguid { get; set; }
    }
}
