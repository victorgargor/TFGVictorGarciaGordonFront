namespace EjerciciosVictorAPI.DTOs
{
    /// <summary>
    /// DTO que representa un permiso dentro del sistema.
    /// </summary>
    public class PermisoDTO
    {
        /// <summary>
        /// Identificador único del permiso.
        /// Se genera automáticamente como un GUID si no se proporciona.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Nombre del permiso.
        /// </summary>
        public string Nombre { get; set; } = null!;
    }
}
