namespace BlazorAppVictor.Entidades
{
    /// <summary>
    /// Representa un usuario en el sistema.
    /// </summary>
    public class Usuario
    {
        // Identificador único del usuario
        public int Id { get; set; }

        // Nombre completo del usuario
        public string Nombre { get; set; } = string.Empty;

        // Dirección de correo electrónico del usuario
        public string Email { get; set; } = string.Empty;

        // Contraseña del usuario (debería ser almacenada de forma segura/hasheada en producción)
        public string Password { get; set; } = string.Empty;

        // Perfil o rol asignado al usuario (ej: Admin, Usuario)
        public string Perfil { get; set; } = string.Empty;

        // Fecha en la que el usuario fue registrado en el sistema
        public DateTime FechaRegistro { get; set; }
    }
}
