namespace BlazorAppVictor.DTOs
{
    /// <summary>
    /// DTO que representa la información básica de un usuario, como su ID y correo electrónico.
    /// </summary>
    public class UsuarioDTO
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// Correo electrónico del usuario.
        /// </summary>
        public string Email { get; set; } = null!;
    }
}
