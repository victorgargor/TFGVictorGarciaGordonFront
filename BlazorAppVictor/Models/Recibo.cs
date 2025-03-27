using System.ComponentModel.DataAnnotations;

namespace BlazorAppVictor.Models
{
    public class Recibo
    {
        [Required(ErrorMessage = "El número de recibo es requerido.")]
        public string NumeroRecibo { get; set; }

        [Required(ErrorMessage = "El importe es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El importe debe ser mayor a 0.")]
        public decimal Importe { get; set; }

        [Required(ErrorMessage = "La fecha de emisión es requerida.")]
        public DateTime FechaEmision { get; set; }

        [Required(ErrorMessage = "El DNI del cliente es requerido.")]
        public string ClienteDNI { get; set; }
    }
}
