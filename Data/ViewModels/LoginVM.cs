using System.ComponentModel.DataAnnotations;

namespace WMS_Nikkosoft.Data.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "Nombre de usuario ")]
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
