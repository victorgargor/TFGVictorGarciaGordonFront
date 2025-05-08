using System.IdentityModel.Tokens.Jwt;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorAppVictor.Helpers;

namespace BlazorAppVictor.Auth
{
    /// <summary>
    /// Servicio responsable de manejar el token JWT almacenado en localStorage
    /// y extraer los claims de autenticación.
    /// </summary>
    public class TokenService
    {
        // Servicio de JavaScript para acceder al localStorage
        private readonly IJSRuntime _jsRuntime;

        /// <summary>
        /// Constructor del TokenService.
        /// </summary>
        /// <param name="jsRuntime">Instancia de IJSRuntime para operaciones JS.</param>
        public TokenService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Obtiene el token JWT almacenado en localStorage.
        /// </summary>
        /// <returns>Token en formato string, o null si no existe.</returns>
        public async Task<string> ObtenerToken()
        {
            // Recupero el token desde el LocalStorage usando la clave definida
            var token = await _jsRuntime.ObtenerDeLocalStorage("TOKENKEY");
            return token?.ToString();
        }

        /// <summary>
        /// Obtiene los claims extraídos del token JWT almacenado.
        /// </summary>
        /// <returns>Lista de claims si el token es válido; lista vacía si no hay token.</returns>
        public async Task<List<Claim>> ObtenerClaimsDesdeToken()
        {
            // Obtengo el token desde LocalStorage
            var token = await ObtenerToken();

            // Devuelvo una lista vacía si no hay token
            if (string.IsNullOrEmpty(token)) return new List<Claim>();

            // Extraigo los claims del token JWT
            var claims = ParsearClaimsDelJWT(token);
            return claims.ToList();
        }

        // Método auxiliar para deserializar el token y extraer los claims
        private IEnumerable<Claim> ParsearClaimsDelJWT(string token)
        {
            // Creo el manejador de JWT
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            // Deserializo el token y accede a sus claims
            var tokenDeserializado = jwtSecurityTokenHandler.ReadJwtToken(token);
            return tokenDeserializado.Claims;
        }
    }
}
