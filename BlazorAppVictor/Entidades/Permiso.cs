using System.ComponentModel.DataAnnotations;

namespace EjerciciosVictorAPI.Entidades
{
    /// <summary>
    /// Entidad que representa un permiso dentro del sistema.
    /// Los permisos son utilizados para controlar el acceso a ciertas funcionalidades.
    /// </summary>
    public class Permiso
    {
        /// <summary>
        /// Identificador único del permiso.
        /// Este valor es generado automáticamente como un GUID si no se proporciona.
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Nombre descriptivo del permiso.
        /// Este campo es obligatorio.
        /// </summary>
        [Required]
        public string Nombre { get; set; } = null!;

        /// <summary>
        /// Lista de relaciones entre roles y permisos (rol-permiso).
        /// Esta propiedad es utilizada para gestionar qué permisos tiene asignados cada rol.
        /// </summary>
        public List<RolPermiso> RolPermisos { get; set; } = new();
    }
}
