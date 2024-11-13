using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlogAngular.Api.Helpers
{
    public static class TokenHelper
    {
        public static string GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Giả định userId nằm trong claim với Type là "sub" hoặc "nameidentifier"
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");

            return userIdClaim?.Value;
        }

    }
}
