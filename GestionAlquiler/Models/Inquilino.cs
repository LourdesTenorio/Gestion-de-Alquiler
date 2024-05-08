using System.ComponentModel.DataAnnotations;

namespace GestionAlquiler.Models
{
    public class Inquilino
    {
        [Display(Name = "Codigo del inquilino")] public int idInquilino { get; set; }
        [Required(ErrorMessage = "El nombre del inquilino es obligatorio.")]
        [MinLength(10, ErrorMessage = "El nombre del inquilino debe tener al menos 10 caracteres.")]
        [Display(Name = "Nombre")] public string nombre { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener exactamente 8 dígitos.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe contener solo números.")]
        [Display(Name = "DNI")] public string dni {get; set;}
        [Display(Name = "Telefono")] public string telefono { get; set; }
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [Display(Name = "Email")] public string email { get; set; }

    }
}
