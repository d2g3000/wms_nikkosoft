using Microsoft.EntityFrameworkCore;

namespace WMS_Nikkosoft.Models
{
    [Keyless]
    public class EntregaReng
    {
        public int? reng_num { get; set; }
        public string? doc_num { get; set; }
        public string? co_art { get; set; }
        public string? des_art { get; set; }
        public decimal total_art { get; set; }

        public Guid? rowguid { get; set; }
    }
}
