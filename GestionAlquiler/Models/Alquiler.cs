using System.ComponentModel.DataAnnotations;

namespace GestionAlquiler.Models
{
    public class Alquiler
    {
        [Display(Name = "Codigo de alquiler")] public int idAlquiler { set; get; }
        [Display(Name="Inquilino")]public Inquilino refInquilino { set; get; }
        [Display(Name = "Numero de Habitación")] public Habitacion refHabitacion { set; get; }
        [Required(ErrorMessage = "La Fecha de Inicio es obligatoria.")]

        [Display(Name = "Fecha de ingreso")] public DateTime FechaInicio { set; get; }
        [Required(ErrorMessage = "La Duración del Alquiler es obligatoria.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Por favor, ingrese solo números.")]
        [Range(1, int.MaxValue, ErrorMessage = "La Duración del Alquiler debe ser mayor o igual a un día.")]
        [Display(Name = "Duracion de Alquiler")] public int Duracion { set; get; }
        [Display(Name = "Pago")] public decimal MontoTotal { set; get; }
        [Display(Name = "Estado de pago")] public string EstadoPago { set; get; }
    }
}
