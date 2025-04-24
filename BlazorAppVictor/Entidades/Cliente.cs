using System.ComponentModel.DataAnnotations;
using BlazorAppVictor.Validations;

namespace BlazorAppVictor.Entidades
{
    /// <summary>
    /// Representa a un cliente dentro del sistema de gestión de recibos.
    /// </summary>
    public class Cliente
    {
        /// <summary>
        /// Identificador único del cliente (PK).
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Propiedad para el DNI del cliente.
        /// </summary>
        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [ValidacionDNI]
        public required string DNI { get; set; }

        /// <summary>
        /// Propiedad para el nombre del cliente.
        /// </summary>
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        public required string Nombre { get; set; }

        /// <summary>
        /// Primer apellido del cliente (opcional).
        /// </summary>
        [StringLength(50, ErrorMessage = "El primer apellido no puede tener más de 50 caracteres.")]
        public string? Apellido1 { get; set; }

        /// <summary>
        /// Segundo apellido del cliente (opcional).
        /// </summary>
        [StringLength(50, ErrorMessage = "El segundo apellido no puede tener más de 50 caracteres.")]
        public string? Apellido2 { get; set; }

        /// <summary>
        /// Propiedad para el tipo de cliente (REGISTRADO o SOCIO).
        /// </summary>
        [Required(ErrorMessage = "El tipo de cliente es obligatorio.")]
        public required TipoCliente Tipo { get; set; }

        /// <summary>
        /// Propiedad opcional para la cuota máxima del cliente (solo para clientes de tipo REGISTRADO).
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "La cuota máxima debe ser un número positivo.")]
        public decimal? CuotaMaxima { get; set; }

        /// <summary>
        /// Propiedad para la fecha de alta del cliente.
        /// </summary>
        [Required(ErrorMessage = "La fecha de alta es obligatoria.")]
        public required DateTime FechaAlta { get; set; }

        /// <summary>
        /// Propiedad para la fecha de baja del cliente (opcional).
        /// </summary>
        [ValidacionFechaBaja]
        public DateTime? FechaBaja { get; set; }

        /// <summary>
        /// Lista de recibos asociados al cliente.
        /// </summary>
        public List<Recibo> Recibos { get; set; } = new List<Recibo>();
    }
}
