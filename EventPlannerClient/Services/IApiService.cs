using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerClient.Services
{
    public interface IApiService
    {
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken);
        public void SetTokens(string jwt, string refreshToken); 
    }
}
