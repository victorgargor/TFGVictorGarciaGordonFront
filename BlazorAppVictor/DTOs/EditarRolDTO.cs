namespace BlazorAppVictor.DTOs
{
    /// <summary>
    /// DTO utilizado para asignar o remover un rol a/de un usuario.
    /// </summary>
    public class EditarRolDTO
    {
        /// <summary>
        /// Identificador del usuario al que se desea asignar o quitar el rol.
        /// </summary>
        public string UsuarioId { get; set; } = null!;

        /// <summary>
        /// Nombre del rol que se asignará o removerá del usuario.
        /// </summary>
        public string Rol { get; set; } = null!;
    }
}
