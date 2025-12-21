using EventPlannerLibrary;
using EventPlannerLibrary.RequestDTOs;
using EventPlannerLibrary.SharedDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerClient.Services
{
    public class AuthorizationService: IAuthorizationService
    {
        private IApiService _apiService;
        private string _login;
        private bool _isAuthorized;
        public event Action Authorized;
        public string Login 
        { 
            get => _login; 
            private set
            {
                if (_login != value) 
                    _login = value;
            } 
        }
        
        public AuthorizationService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<bool> Authorize(string login, string password)
        {
            var requestData = new AuthorizationUserRequest() { Login = login, Password = password };
            if (!Validator<AuthorizationUserRequest>.IsValid(requestData))
                return false;

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Authorization/login") {Content = JsonContent.Create(requestData)};
            var response = await _apiService.SendAsync(requestMessage, new CancellationToken());
            if (!response.IsSuccessStatusCode)
                return false;

            var responseData = (await response.Content.ReadFromJsonAsync<ApiResponse<JwtDTO>>()).Data;
            if (responseData is null)
                return false;

            _apiService.SetTokens(responseData.JWT, responseData.Refresh);
            Login = responseData.Login;
            Authorized?.Invoke();
            return true;
        }

        public async Task<bool> Register(string login, string password, string confirmPassword)
        {
            if (password != confirmPassword)
                return false;

            var requestData = new AuthorizationUserRequest() { Login = login, Password = password };
            if (!Validator<AuthorizationUserRequest>.IsValid(requestData))
                return false;

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Authorization/register") { Content = JsonContent.Create(requestData) };
            var response = await _apiService.SendAsync(requestMessage, new CancellationToken());
            if (!response.IsSuccessStatusCode)
                return false;

            var responseData = (await response.Content.ReadFromJsonAsync<ApiResponse<JwtDTO>>()).Data;
            if (responseData is null)
                return false;

            _apiService.SetTokens(responseData.JWT, responseData.Refresh);
            Login = responseData.Login;
            Authorized?.Invoke();
            return true;
        }
        public void Logout()
        {
            //Пока заглушка сделаю как проснусь
        }
    }
}
