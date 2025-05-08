namespace EjerciciosVictorAPI.DTOs
{
    /// <summary>
    /// DTO que representa la relación entre un rol y un permiso.
    /// Se utiliza para asignar o remover permisos de roles.
    /// </summary>
    public class RolPermisoDTO
    {
        /// <summary>
        /// Identificador único del rol.
        /// </summary>
        public string RolId { get; set; } = string.Empty;

        /// <summary>
        /// Identificador único del permiso que se va a asignar o remover del rol.
        /// </summary>
        public string PermisoId { get; set; } = string.Empty;
    }
}
