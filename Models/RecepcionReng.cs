using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_Nikkosoft.Models
{
    [Keyless]
    public class RecepcionReng
    {
        public int? reng_num { get; set; }
        public string? doc_num { get; set; }
        public string? co_art { get; set; }
        [ForeignKey("co_art")]
        public Articulo? articulo { get; set; }
        public string? des_art { get; set; }
        public decimal total_art { get; set; }
        public string? co_alma { get; set; }
        public decimal cost_unit { get; set; }
        public Guid? rowguid { get; set; }
    }
}
