using WMS_Nikkosoft.Models;

namespace WMS_Nikkosoft.Data.ViewModels
{
    public class ComboBoxCasoVM
    {

        public ComboBoxCasoVM()
        {
            almacen = new List<Almacen>();

        }

        public List<Almacen> almacen { get; set; }

    }
}

