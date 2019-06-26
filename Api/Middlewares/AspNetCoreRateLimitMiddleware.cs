using AspNetCoreRateLimit;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace Api.Middlewares
{
    public class AspNetCoreRateLimitMiddleware
    {
        private readonly ClientRateLimitOptions _options;
        private readonly IClientPolicyStore _clientPolicyStore;
        private readonly RequestDelegate next;

        public AspNetCoreRateLimitMiddleware(
            RequestDelegate next,
            IOptions<ClientRateLimitOptions> optionsAccessor,
            IClientPolicyStore clientPolicyStore)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            _options = optionsAccessor.Value;
            _clientPolicyStore = clientPolicyStore;
        }

        public async Task Invoke(HttpContext context)
        {
            var clientId = context.User.Claims.FirstOrDefault(c => c.Type == "client_id") != null
                   ? context.User.Claims.FirstOrDefault(c => c.Type == "client_id").Value
                   : "anonimo";

            var totalLimit = int.Parse(context.User.Claims.FirstOrDefault(c => c.Type == "client_AspNetRateLimit24h") != null
                    ? context.User.Claims.FirstOrDefault(c => c.Type == "client_AspNetRateLimit24h").Value
                    : "5");

            if (clientId != "anonimo")
            {
                var rule = new ClientRateLimitPolicy()
                {
                    ClientId = clientId,
                    Rules = new List<RateLimitRule>()
                    {
                        new RateLimitRule
                        {
                            Endpoint = "*",
                            Period = "24h",
                            Limit = totalLimit
                        }
                    }
                };
                await _clientPolicyStore.SetAsync($"{_options.ClientPolicyPrefix}_{rule.ClientId}", new ClientRateLimitPolicy { ClientId = rule.ClientId, Rules = rule.Rules }).ConfigureAwait(false);
            }

            await next(context);
        }
    }
}
