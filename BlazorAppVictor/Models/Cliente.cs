using System.ComponentModel.DataAnnotations;

namespace BlazorAppVictor.Models
{
    public class Cliente
    {
        /// <summary>
        /// Propiedad para el DNI del cliente.
        /// </summary>
        [Key]
        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "El DNI debe tener 9 caracteres.")]
        [RegularExpression(@"^\d{8}[A-Za-z]$", ErrorMessage = "El DNI debe tener 8 números seguidos de una letra.")]
        public string DNI { get; set; }

        /// <summary>
        /// Propiedad para el nombre del cliente.
        /// </summary>
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        public string? Nombre { get; set; }

        /// <summary>
        /// Propiedad para los apellidos del cliente.
        /// </summary>
        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(100, ErrorMessage = "Los apellidos no pueden tener más de 100 caracteres.")]
        public string? Apellidos { get; set; }

        /// <summary>
        /// Propiedad para el tipo de cliente (REGISTRADO o SOCIO).
        /// </summary>
        [Required(ErrorMessage = "El tipo de cliente es obligatorio.")]
        public TipoCliente Tipo { get; set; }

        /// <summary>
        /// Propiedad opcional para la cuota máxima del cliente (solo para clientes de tipo REGISTRADO).
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "La cuota máxima debe ser un número positivo.")]
        public decimal? CuotaMaxima { get; set; }

        /// <summary>
        /// Propiedad para la fecha de alta del cliente.
        /// </summary>
        [Required(ErrorMessage = "La fecha de alta es obligatoria.")]
        public DateTime FechaAlta { get; set; }

        /// <summary>
        /// Lista de recibos asociados al cliente.
        /// </summary>
        public List<Recibo> Recibos { get; set; } = new List<Recibo>();

    }
}
