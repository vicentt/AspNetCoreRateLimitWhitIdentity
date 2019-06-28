using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    public class ApiClientService
    {
        private readonly HttpClient client;

        public ApiClientService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<HttpResponseMessage> CallApi()
        {
            HttpResponseMessage response
              = await client.GetAsync("/api/values");

            return response != null ? response : null;
        }
    }
}
