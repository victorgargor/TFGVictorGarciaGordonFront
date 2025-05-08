using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using BlazorAppVictor.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BlazorAppVictor.Auth
{
    /// <summary>
    /// Proveedor de autenticación personalizado para Blazor que utiliza JWT almacenado en LocalStorage.
    /// </summary>
    public class ProveedorAutenticacionJWT : AuthenticationStateProvider, ILoginService
    {
        // Referencia al servicio JS para interactuar con el LocalStorage
        private readonly IJSRuntime js;

        // Cliente HTTP utilizado para configurar el encabezado de autorización
        private readonly HttpClient httpClient;

        /// <summary>
        /// Constructor del proveedor de autenticación.
        /// </summary>
        /// <param name="js">Instancia de IJSRuntime para acceso al LocalStorage.</param>
        /// <param name="httpClient">Instancia de HttpClient para configuración del token.</param>
        public ProveedorAutenticacionJWT(IJSRuntime js, HttpClient httpClient)
        {
            this.js = js;
            this.httpClient = httpClient;
        }

        // Clave constante para acceder al token en el LocalStorage
        public static readonly string TOKENKEY = "TOKENKEY";

        // Representa un usuario anónimo (sin claims)
        private AuthenticationState Anonimo =>
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        /// <summary>
        /// Obtiene el estado actual de autenticación del usuario.
        /// </summary>
        /// <returns>Estado de autenticación basado en el JWT o estado anónimo.</returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Intento recuperar el token desde LocalStorage
            var token = await js.ObtenerDeLocalStorage(TOKENKEY);

            // Si no hay token, el usuario está anónimo
            if (token is null)
            {
                return Anonimo;
            }

            // Si hay token, construyo el estado autenticado
            return ConstruirAuthenticationState(token.ToString());
        }

        // Construye un AuthenticationState válido a partir del token JWT
        private AuthenticationState ConstruirAuthenticationState(string token)
        {
            // Configuro el encabezado Authorization en el HttpClient
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", token);

            // Obtengo los claims del token
            var claims = ParsearClaimsDelJWT(token);

            // Creo el estado de autenticación con los claims obtenidos
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
        }

        // Método auxiliar que extrae los claims del token JWT
        private IEnumerable<Claim> ParsearClaimsDelJWT(string token)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            // Deserializo el token en formato JWT
            var tokenDeserializado = jwtSecurityTokenHandler.ReadJwtToken(token);

            // Devuelvo la lista de claims del token
            return tokenDeserializado.Claims;
        }

        /// <summary>
        /// Inicia sesión guardando el token en el LocalStorage y actualizando el estado de autenticación.
        /// </summary>
        /// <param name="token">Token JWT válido.</param>
        public async Task Login(string token)
        {
            // Guardo el token en LocalStorage
            await js.GuardarEnLocalStorage(TOKENKEY, token);

            // Construyo el nuevo estado autenticado
            var authState = ConstruirAuthenticationState(token);

            // Notifico a Blazor que el estado de autenticación cambió
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        /// <summary>
        /// Cierra sesión eliminando el token del LocalStorage y limpiando el estado.
        /// </summary>
        public async Task Logout()
        {
            // Elimino el token del LocalStorage
            await js.RemoverDelLocalStorage(TOKENKEY);

            // Elimino el encabezado Authorization del HttpClient
            httpClient.DefaultRequestHeaders.Authorization = null;

            // Notifica que el usuario ahora es anónimo
            NotifyAuthenticationStateChanged(Task.FromResult(Anonimo));
        }
    }
}