using System.IdentityModel.Tokens.Jwt;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorAppVictor.Helpers;

namespace BlazorAppVictor.Auth
{
    public class TokenService
    {
        private readonly IJSRuntime _jsRuntime;

        public TokenService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        // Recupera el token de localStorage
        public async Task<string> ObtenerToken()
        {
            var token = await _jsRuntime.ObtenerDeLocalStorage("TOKENKEY");
            return token?.ToString();
        }

        // Decodifica el token y obtiene los claims
        public async Task<List<Claim>> ObtenerClaimsDesdeToken()
        {
            var token = await ObtenerToken();
            if (string.IsNullOrEmpty(token)) return new List<Claim>();

            var claims = ParsearClaimsDelJWT(token);
            return claims.ToList();
        }

        // Método para parsear los claims del JWT
        private IEnumerable<Claim> ParsearClaimsDelJWT(string token)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenDeserializado = jwtSecurityTokenHandler.ReadJwtToken(token);
            return tokenDeserializado.Claims;
        }
    }
}

