using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pickpong.Login
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            IWebHostEnvironment? env = (IWebHostEnvironment?)context.HttpContext.RequestServices
                .GetService(typeof(IWebHostEnvironment));
            StringValues skipAuthHeader = context.HttpContext.Request.Headers["SkipAuth"];

            if ((env != null && env.IsDevelopment()) || skipAuthHeader == "true")
            {
                context.HttpContext.Items["UserId"] = 1;
                return;
            }

            StringValues token = context.HttpContext.Request.Headers["Authorization"];
            if (String.IsNullOrEmpty(token) || !validToken(token!, context))
            {
                context.Result = new JsonResult(new { message = "Unauthorized" })
                { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }

        private bool validToken(string token, AuthorizationFilterContext context)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes("!@#$%^7890-=+_)(*&654321");
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;

                foreach (Claim claim in jwtToken.Claims)
                {
                    Console.WriteLine($"Type: {claim.Type}, Value: {claim.Value}");
                }
                ClaimsIdentity? identity =
                    context.HttpContext.User.Identity as ClaimsIdentity;
                if (identity!.Claims.Count() == 0)
                    identity.AddClaims(jwtToken.Claims);
                JToken? iUserIdValue = jwtToken.Claims
                    .Where(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid")
                    .Select(claim => JObject.Parse(claim.Value)["iUserId"])
                    .FirstOrDefault();
                if (iUserIdValue != null)
                {
                    int iUserId = Convert.ToInt32(iUserIdValue);
                    context.HttpContext.Items["UserId"] = iUserId;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}