using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace dotNetflix.Models
{
    public class ClaimsTransformer : IClaimsTransformation
    {


        /// <summary>
        /// ClaimsTransformer
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            principal.Identities.First().AddClaim(new Claim("now", DateTime.Now.ToString()));

            return Task.FromResult(principal);
        }
    }
}