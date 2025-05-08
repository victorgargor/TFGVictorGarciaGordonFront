using Microsoft.AspNetCore.Identity;

namespace EjerciciosVictorAPI.Entidades
{
    /// <summary>
    /// Entidad que representa la relación entre un rol y un permiso.
    /// Permite asignar permisos específicos a roles en el sistema.
    /// </summary>
    public class RolPermiso
    {
        /// <summary>
        /// Identificador del rol.
        /// Este valor corresponde al Id del rol asociado.
        /// </summary>
        public string RolId { get; set; } = null!;

        /// <summary>
        /// Rol asociado a este permiso.
        /// Relacionado con la entidad IdentityRole de ASP.NET Identity.
        /// </summary>
        public IdentityRole Rol { get; set; } = null!;

        /// <summary>
        /// Identificador único del permiso.
        /// Este valor corresponde al Id del permiso asociado.
        /// </summary>
        public Guid PermisoId { get; set; }

        /// <summary>
        /// Permiso asociado al rol.
        /// Relacionado con la entidad Permiso en el sistema.
        /// </summary>
        public Permiso Permiso { get; set; } = null!;
    }
}
