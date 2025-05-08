namespace BlazorAppVictor.Auth
{
    /// <summary>
    /// Interfaz que define los métodos para el manejo de autenticación del usuario en la aplicación Blazor.
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// Realiza el inicio de sesión almacenando el token JWT en el sistema de autenticación.
        /// </summary>
        /// <param name="token">Token JWT válido obtenido del backend.</param>
        Task Login(string token);

        /// <summary>
        /// Realiza el cierre de sesión eliminando el token almacenado.
        /// </summary>
        Task Logout(); 
    }
}
