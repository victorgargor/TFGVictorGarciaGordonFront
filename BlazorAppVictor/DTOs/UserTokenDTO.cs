namespace BlazorAppVictor.DTOs
{
    /// <summary>
    /// DTO que representa el token JWT generado para un usuario autenticado.
    /// </summary>
    public class UserTokenDTO
    {
        /// <summary>
        /// Token JWT emitido al usuario.
        /// Este token debe ser enviado en las siguientes peticiones para autenticación.
        /// </summary>
        public string Token { get; set; } = null!;

        /// <summary>
        /// Fecha y hora de expiración del token.
        /// </summary>
        public DateTime Expiration { get; set; }
    }
}
