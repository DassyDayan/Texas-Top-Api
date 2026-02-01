using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pickpong.Login
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private const string SecretKey = "!@#$%^7890-=+_)(*&654321";
        private const string AuthorizationHeader = "Authorization";
        private const string UserIdClaimUri = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid";
        private const string UserIdKey = "UserId";

        public CustomAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string? token = context.Request.Headers[AuthorizationHeader].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(token)
                && TryValidateToken(token, out int userId, out IEnumerable<Claim> claims))
            {
                if (context.User.Identity is ClaimsIdentity identity && !identity.Claims.Any())
                {
                    identity.AddClaims(claims);
                }
                context.Items[UserIdKey] = userId;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = "Unauthorized" });
                return;
            }
            await _next(context);
        }

        private bool TryValidateToken(string token, out int userId, out IEnumerable<Claim> claims)
        {
            userId = 0;
            claims = Enumerable.Empty<Claim>();
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(SecretKey);
                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
                tokenHandler.ValidateToken(token, validationParameters,
                    out SecurityToken validatedToken);
                if (validatedToken is not JwtSecurityToken jwtToken)
                    return false;
                claims = jwtToken.Claims;
                Claim? userIdClaim = jwtToken.Claims
                    .FirstOrDefault(c => c.Type == UserIdClaimUri);
                if (userIdClaim == null) return false;
                JObject json = JObject.Parse(userIdClaim.Value);
                JToken? userIdToken = json["iUserId"];
                if (userIdToken != null && int.TryParse(userIdToken.ToString(), out userId))
                    return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AUTH ERROR] {ex.Message}");
            }
            claims = Enumerable.Empty<Claim>();
            return false;
        }
    }
}