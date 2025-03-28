using System.ComponentModel.DataAnnotations;

namespace BlazorAppVictor.Models
{
    /// <summary>
    /// Representa un recibo asociado a un cliente.
    /// </summary>
    public class Recibo
    {
        /// <summary>
        /// Número de recibo único que actúa como PK (primary key).
        /// </summary>
        [Key]
        [Required(ErrorMessage = "El número de recibo es requerido")]
        public string NumeroRecibo { get; set; }

        /// <summary>
        /// Importe del recibo.
        /// </summary>
        [Required(ErrorMessage = "Se requiere importe")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El importe debe ser mayor a 0")]
        public decimal Importe { get; set; }

        /// <summary>
        /// Fecha de emisión del recibo.
        /// </summary>
        [Required(ErrorMessage = "Se requiere la fecha de emisión")]
        public DateTime FechaEmision { get; set; }

        /// <summary>
        /// DNI del cliente al que pertenece el recibo.
        /// </summary>
        [Required(ErrorMessage = "Se requiere el DNI del cliente correspondiente")]
        public string ClienteDNI { get; set; }

        /// <summary>
        /// Propiedad de navegación con el cliente asociado al recibo.
        /// </summary>
        public Cliente? Cliente { get; set; }
    }
}
