using System.IdentityModel.Tokens.Jwt;

namespace catch_up_backend.Helpers
{
    public static class TokenHelper
    {
        public static Guid GetUserIdFromTokenInRequest(HttpRequest request)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(
                request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim()
            );
            var userId = Guid.Parse(jwtToken.Claims.First(c => c.Type == "nameid").Value);

            return userId;
        }
    }
}
