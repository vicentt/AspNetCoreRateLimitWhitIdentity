using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;


namespace Api.AspNetCoreRateLimitExtensions
{
    public class RateLimitConfigurationExtension : RateLimitConfiguration
    {
        public RateLimitConfigurationExtension(
            IHttpContextAccessor httpContextAccessor,
            IOptions<IpRateLimitOptions> ipOptions,
            IOptions<ClientRateLimitOptions> clientOptions) : base(httpContextAccessor, ipOptions, clientOptions)
        {
            RegisterResolvers();
        }

        protected override void RegisterResolvers()
        {
            ClientResolvers.Add(new IdentityResolveContributor(HttpContextAccessor));
        }
    }
}
