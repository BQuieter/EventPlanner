using EventPlannerLibrary;
using EventPlannerLibrary.RequestDTOs;
using EventPlannerLibrary.SharedDTOs;
using System.Net.Http.Json;

HttpClient client = new HttpClient();




async void TestRegister()
{
    AuthorizationUserRequest test = new() { Login = "Kolya1234", Password = "123456" };
    var response = client.PostAsJsonAsync<AuthorizationUserRequest>("https://localhost:7100/api/Authorization/register", test);
    var apiResponse = await response.Result.Content.ReadFromJsonAsync<ApiResponse<JwtDTO>>();
    if (apiResponse?.Success == true)
    {
        Console.WriteLine(apiResponse.Data.Login);
        Console.WriteLine(apiResponse.Data.JWT);
    }
    Console.ReadLine();
}

async void TestAuthorizatiton()
{
    AuthorizationUserRequest test = new() { Login = "Kolya1234", Password = "123456" };
    var response = client.PostAsJsonAsync<AuthorizationUserRequest>("https://localhost:7100/api/Authorization/login", test);
    var apiResponse = await response.Result.Content.ReadFromJsonAsync<ApiResponse<JwtDTO>>();
    if (apiResponse?.Success == true)
    {
        Console.WriteLine(apiResponse.Data.Login);
        Console.WriteLine(apiResponse.Data.JWT);
    }
    Console.ReadLine();
}


