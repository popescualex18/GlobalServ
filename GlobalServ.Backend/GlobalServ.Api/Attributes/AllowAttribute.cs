using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GlobalServ.Common.Enum;
using GlobalServ.Core.Implementations;

namespace GlobalServ.Api.Attributes
{
    public class AllowAttribute : Attribute, IAuthorizationFilter
    {
        private readonly TokenHelper _tokenHelper;

        public RolesEnum Roles { get; set; }
        public AllowAttribute(RolesEnum roles)
        {
            Roles = roles;
            _tokenHelper = new TokenHelper();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            context.HttpContext.Request.Headers.TryGetValue("Authorization", out var values);
            // Your custom authorization logic here
            if (!IsAuthorized(values.FirstOrDefault()))
            {
                context.Result = new UnauthorizedResult();
            }
        }

        private bool IsAuthorized(string token)
        {
            if (token == null)
            {
                return false;
            }
            var claims = _tokenHelper.GetClaims(token);
            foreach (var claim in claims)
            {
                var a = claim.Type;
                var b = claim.Value;
            }
            var rolesClaims = claims.FirstOrDefault(x => x.Type == "role")?.Value;
            if (rolesClaims == null)
            {
                return false;
            }
            int role = int.Parse(rolesClaims);
            return ((int)Roles & role) == role; // For demonstration purposes
        }
    }
}
