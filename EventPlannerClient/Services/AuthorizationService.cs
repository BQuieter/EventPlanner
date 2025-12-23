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

        public async Task<ServiceResponse<string?>> Authorize(string login, string password)
        {
            var requestData = new AuthorizationUserRequest() { Login = login, Password = password };
            if (!Validator<AuthorizationUserRequest>.IsValid(requestData))
                return new ServiceResponse<string?>() {IsSuccessed = false ,ErrorMessage = "Некорректные данные" };

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Authorization/login") {Content = JsonContent.Create(requestData)};
            var response = await _apiService.SendAsync(requestMessage, new CancellationToken());

            var responseData = await response.Content.ReadFromJsonAsync<ApiResponse<JwtDTO>>();
            if (!response.IsSuccessStatusCode && responseData is not null)
                return new ServiceResponse<string?>() { IsSuccessed = false, ErrorMessage = responseData.Error, ErrorCode = responseData.ErrorCode };
            else if (!response.IsSuccessStatusCode || (response.IsSuccessStatusCode && responseData.Data is null))
                return new ServiceResponse<string?>() { IsSuccessed = false, ErrorMessage = "Неизвестная ошибка", ErrorCode = (int)response.StatusCode };

            _apiService.SetTokens(responseData.Data.JWT, responseData.Data.Refresh);
            Login = responseData.Data.Login;
            Authorized?.Invoke();
            return new ServiceResponse<string?>() { IsSuccessed = true};
        }

        public async Task<ServiceResponse<string?>> Register(string login, string password, string confirmPassword)
        {
            if (password != confirmPassword)
                return new ServiceResponse<string?>() { IsSuccessed = false, ErrorMessage = "Пароли не совпадают" };

            var requestData = new AuthorizationUserRequest() { Login = login, Password = password };
            if (!Validator<AuthorizationUserRequest>.IsValid(requestData))
                return new ServiceResponse<string?>() { IsSuccessed = false, ErrorMessage = "Некорректные данные" };

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Authorization/register") { Content = JsonContent.Create(requestData) };
            var response = await _apiService.SendAsync(requestMessage, new CancellationToken());

            var responseData = await response.Content.ReadFromJsonAsync<ApiResponse<JwtDTO>>();
            if (!response.IsSuccessStatusCode && responseData is not null)
                return new ServiceResponse<string?>() { IsSuccessed = false, ErrorMessage = responseData.Error, ErrorCode = responseData.ErrorCode };
            else if (!response.IsSuccessStatusCode || (response.IsSuccessStatusCode && responseData.Data is null))
                return new ServiceResponse<string?>() { IsSuccessed = false, ErrorMessage = "Неизвестная ошибка", ErrorCode = (int)response.StatusCode };

            _apiService.SetTokens(responseData.Data.JWT, responseData.Data.Refresh);
            Login = responseData.Data.Login;
            Authorized?.Invoke();
            return new ServiceResponse<string?>() { IsSuccessed = true };
        }
        public void Logout()
        {
            //Пока заглушка сделаю как проснусь
        }
    }
}
