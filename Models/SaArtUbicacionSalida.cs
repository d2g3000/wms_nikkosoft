using Microsoft.EntityFrameworkCore;

namespace WMS_Nikkosoft.Models
{
    [Keyless]
    public class SaArtUbicacionSalida
    {
        public Guid rowguid_reng { get; set; }
        public string? co_art { get; set; }
        public string? co_alma { get; set; }
        public int reng_num { get; set; }
        public decimal cantidad { get; set; }

        public string? ubicacion { get; set; }

        public Guid rowguid { get; set; }
    }
}
