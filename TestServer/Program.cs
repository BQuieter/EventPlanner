using EventPlannerLibrary;
using EventPlannerLibrary.RequestDTOs;
using EventPlannerLibrary.ResponseDTOs;
using System.Net.Http.Json;

AuthorizationUserRequest test = new() { Login = "Diman123", Password = "123456789" };
HttpClient client = new HttpClient();

var response = client.PostAsJsonAsync<AuthorizationUserRequest>("https://localhost:7100/api/Authorization/login", test);
var apiResponse = await response.Result.Content.ReadFromJsonAsync<ApiResponse<JWTResponse>>();
if (apiResponse?.Success == true)
{
    Console.WriteLine(apiResponse.Data.Login);
    Console.WriteLine(apiResponse.Data.JWT);
}
Console.ReadLine();