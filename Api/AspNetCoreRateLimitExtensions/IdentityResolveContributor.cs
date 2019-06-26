using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.AspNetCoreRateLimitExtensions
{
    public class IdentityResolveContributor : IClientResolveContributor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IdentityResolveContributor(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string ResolveClient()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var clientId =
                httpContext.User.Claims.FirstOrDefault(c => c.Type == "client_id") != null
                ? httpContext.User.Claims.FirstOrDefault(c => c.Type == "client_id").Value
                : "anon";

            return clientId;
        }
    }
}
