using System.ComponentModel.DataAnnotations;

namespace WMS_Nikkosoft.Models
{
    public class Recepcion
    {
        [Key]
        public string? doc_num { get; set; }
        public string? nro_fact { get; set; }
        public string? descrip { get; set; }
        public string? co_prov { get; set; }
        public DateTime fec_emis { get; set; }
        public Guid rowguid { get; set; }
    }
}
