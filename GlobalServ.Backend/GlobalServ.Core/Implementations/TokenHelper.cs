using GlobalServ.Core.Interfaces;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GlobalServ.Core.Implementations
{
    public class TokenHelper : ITokenHelper
    {
        private static ClaimsIdentity ConstructSubject(string userId, string userEmail, string role)
        {
            var claimLists = new List<Claim>
            {
                new(ClaimTypes.Email, userEmail),
                new(ClaimTypes.Name, userId),
                new Claim(ClaimTypes.Role, role)
            };

            return new ClaimsIdentity(claimLists);
        }

        public string GenerateToken(string userId, string userEmail, string role, DateTime? expirationDate, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = ConstructSubject(userId, userEmail, role),
                Expires = expirationDate.HasValue ? expirationDate.Value : DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public static (ClaimsPrincipal, SecurityToken) ValidateToken(string token, string secret, bool validateLifetime = true)
        {
            IdentityModelEventSource.ShowPII = true;

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                //Same Secret key will be used while creating the token
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = validateLifetime,
                ClockSkew = TimeSpan.Zero
            };

            var principal =
                new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out var securityToken);

            return (principal, securityToken);
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token, string secret, bool isRefreshCall = false)
        {
            var (principal, _) = ValidateToken(token, secret, !isRefreshCall);

            return principal;
        }

        public bool IsTokenExpired(string token, string secret)
        {
            try
            {
                GetPrincipalFromToken(token, secret);
            }
            catch (SecurityTokenExpiredException)
            {
                return true;
            }

            return false;
        }

        public IList<Claim> GetClaims(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            return jwtToken != null ? jwtToken.Claims.ToList() : new List<Claim>();

        }
    }
}
