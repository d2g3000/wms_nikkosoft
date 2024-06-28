using System.ComponentModel.DataAnnotations;

namespace WMS_Nikkosoft.Data.ViewModels
{
    public class RegisterVM
    {
        [Display(Name = "Nombre y apellido")]
        [Required(ErrorMessage = "el nombre es requerido")]
        [RegularExpression(@"[a-zA-Z ]{5,30}$",
         ErrorMessage = "Hay caracteres no permitidos")]
        public string FullName { get; set; }
        [Display(Name = "Nombre de usuario")]
        [Required(ErrorMessage = "el nombre es requerido")]
        [RegularExpression(@"[a-zA-Z0-9-_]{5,20}$",
         ErrorMessage = "Hay caracteres no permitidos")]
        public string UserName { get; set; }

        [Display(Name = "Dirección de correo")]
        [Required(ErrorMessage = " Dirección de correo es requerida")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [Display(Name = "Confirme dirección de correo")]
        [Required(ErrorMessage = "Dirección de correo es requerida")]
        [Compare("EmailAddress", ErrorMessage = "La dirección de correo no coincide")]
        [DataType(DataType.EmailAddress)]
        public string ConfirmEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirme password")]
        [Required(ErrorMessage = "Confirmación es requerida")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "El password no coincide")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Cédula de identidad")]
        public int cedula { get; set; }
        [Display(Name = "Rif")]
        [RegularExpression(@"[a-zA-Z0-9\- ]{5,20}$",
         ErrorMessage = "Hay caracteres no permitidos")]
        public string rif { get; set; }
        [Display(Name = "Codigo vendedor")]
        [RegularExpression(@"[a-zA-Z0-9\(\)\- ]{5,100}$",
         ErrorMessage = "Hay caracteres no permitidos")]
        public string CodigoVendedor { get; set; }
        [Display(Name = "Teléfono")]
        [RegularExpression(@"[0-9\(\)\- ]{5,20}$",
         ErrorMessage = "Hay caracteres no permitidos")]
        public string phoneNumber { get; set; }
    }
}
