using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BlazorAppVictor.Converters;

namespace BlazorAppVictor.Entidades
{
    /// <summary>
    /// Representa un recibo asociado a un cliente.
    /// </summary>
    public class Recibo
    {
        /// <summary>
        /// Identificador único del recibo (PK).
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Número de recibo único que actúa como PK (primary key).
        /// </summary>
        [Required(ErrorMessage = "El número de recibo es requerido")]
        public required string NumeroRecibo { get; set; }

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
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime FechaEmision { get; set; }

        /// <summary>
        /// Id del cliente al que pertenece el recibo.
        /// </summary>
        [Required(ErrorMessage = "Se requiere el Id del cliente correspondiente")]
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }

        /// <summary>
        /// Propiedad de navegación con el cliente asociado al recibo.
        /// </summary>
        public Cliente? Cliente { get; set; }
    }
}
