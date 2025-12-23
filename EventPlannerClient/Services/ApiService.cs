using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using EventPlannerLibrary;
using EventPlannerLibrary.SharedDTOs;
using Microsoft.Xaml.Behaviors.Media;

namespace EventPlannerClient.Services
{
    public class ApiService : IApiService
    {
        private HttpClient _httpClient;
        private string _jwt;
        private string _refreshToken;
        private readonly SemaphoreSlim _refreshLock = new(1, 1);
        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7100/api/");
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage? response;
            try
            {
                response = await _httpClient.SendAsync(request, cancellationToken);

                if (response.StatusCode == HttpStatusCode.Unauthorized && _refreshToken is not null)
                {
                    try
                    {
                        await _refreshLock.WaitAsync(cancellationToken);
                        var refreshSuccess = await RefreshTokenAsync();
                        if (refreshSuccess)
                            response = await _httpClient.SendAsync(request, cancellationToken);
                    }
                    finally
                    {
                        _refreshLock.Release();
                    }
                }
                return response;
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex.Message);
                return new HttpResponseMessage() { StatusCode = HttpStatusCode.ServiceUnavailable };
            }
        }
       
        public void SetTokens(string jwt, string refreshToken)
        {
            _jwt = jwt;
            _refreshToken = refreshToken;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _jwt);
        }

        private async Task<bool> RefreshTokenAsync()
        {
            var requestData = new JwtDTO() { JWT = _jwt, Refresh = _refreshToken};
            var response = await _httpClient.PostAsJsonAsync<JwtDTO>("Authorization/refresh", requestData);
            if (!response.IsSuccessStatusCode)
                return false;
            var responseData = await response.Content.ReadFromJsonAsync<ApiResponse<JwtDTO>>();
            if (responseData.Data is null)
                return false;
            SetTokens(responseData.Data.JWT, responseData.Data.Refresh);
            return true;
        }
    }
}
