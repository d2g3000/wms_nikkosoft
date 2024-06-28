using System.ComponentModel.DataAnnotations;

namespace WMS_Nikkosoft.Models
{
    public class Entrega
    {
        [Key]
        public string? doc_num { get; set; }

        public string? descrip { get; set; }
        public string? co_cli { get; set; }
        public DateTime fec_emis { get; set; }
        public Guid rowguid { get; set; }
    }
}
