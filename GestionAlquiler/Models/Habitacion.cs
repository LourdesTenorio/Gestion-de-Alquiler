using System.ComponentModel.DataAnnotations;

namespace GestionAlquiler.Models
{
    public class Habitacion
    {
        [Display(Name = "Codigo del habitación")] public int idHabitacion { set; get; }
        
        [Required(ErrorMessage = "El número de habitación es obligatorio")]
        [Display(Name = "Número de habitación")] public string NumeroHabitacion { set; get; }
        
        [Required(ErrorMessage = "El tipo de habitación es obligatorio")]
        [Display(Name = "Tipo de Habitación")] public string Tipo { set; get; }
        
        [Required(ErrorMessage = "El precio de alquiler es obligatorio")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El precio de alquiler debe ser un número decimal válido")]
        [Display(Name = "Precio")] public decimal PrecioAlquiler { set; get; }
        
        [Required(ErrorMessage = "El estado de la habitación es obligatorio")]
        [Display(Name = "Estado de la habitación")] public string Estado { set; get; }
    }
}
