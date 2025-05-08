namespace BlazorAppVictor.DTOs
{
    /// <summary>
    /// DTO que representa un rol dentro del sistema.
    /// </summary>
    public class RolDTO
    {
        /// <summary>
        /// Identificador único del rol.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// Nombre del rol asignado (por ejemplo, "admin", "usuario", etc.).
        /// </summary>
        public string Nombre { get; set; } = null!;
    }
}
