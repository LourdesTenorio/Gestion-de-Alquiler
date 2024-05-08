using System.ComponentModel.DataAnnotations;

namespace GestionAlquiler.Models
{
    public class Pagos
    {
        [Display(Name = "Codigo de pago")] public int idPago { set; get; }
        [Display(Name = "Codigo del alquiler")] public Alquiler idAlquiler { get; set; }
        [Display(Name = "Fecha de pago")] public DateTime FechaPago { get; set; }
        [Display(Name = "Monto pagado")] public decimal MontoPagado {get;set;}
        [Display(Name = "Metodo de pago")] public string MetodoPago { set; get; }
        [Display(Name = "Estado de pago")] public string Estado { set; get; }
    }
}
