using EventPlannerLibrary;
using EventPlannerLibrary.RequestDTOs;
using EventPlannerLibrary.SharedDTOs;
using System.Net.Http.Json;
using static System.Net.Mime.MediaTypeNames;

HttpClient client = new HttpClient();

string refreshToken = "";
string jwtToken = "";

await TestAuthorizatiton();
await TestGetEvents();
Task.Delay(TimeSpan.FromSeconds(30)).Wait();
await TestGetEvents();

async Task TestRegister()
{
    AuthorizationUserRequest test = new() { Login = "Kolya1234", Password = "123456" };
    var response = client.PostAsJsonAsync<AuthorizationUserRequest>("https://localhost:7100/api/Authorization/register", test);
    var apiResponse = await response.Result.Content.ReadFromJsonAsync<ApiResponse<JwtDTO>>();
    if (apiResponse?.Success == true)
    {
        Console.WriteLine("Register");
        SetAuthToken(apiResponse.Data.JWT, apiResponse.Data.Refresh);
    }
}

async Task TestAuthorizatiton()
{
    AuthorizationUserRequest test = new() { Login = "Kolya1234", Password = "123456" };
    var response = client.PostAsJsonAsync<AuthorizationUserRequest>("https://localhost:7100/api/Authorization/login", test);
    var apiResponse = await response.Result.Content.ReadFromJsonAsync<ApiResponse<JwtDTO>>();
    if (apiResponse?.Success == true)
    {
        Console.WriteLine("Auth");
        SetAuthToken(apiResponse.Data.JWT, apiResponse.Data.Refresh);
    }
}

async Task TestGetEvents()
{
    Console.WriteLine(DateTime.UtcNow);
    try
    {
        var response = await client.GetAsync("https://localhost:7100/api/Events/getevents/2025-12");
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("yee");
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<EventDTO>>>();
            if (result.Data is not null)
                Console.WriteLine(result.Data.Count);
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            Console.WriteLine("Unauthorize");
            await TestRefresh();
            await TestGetEvents();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Краш в GetAsync: {ex.GetType().Name}: {ex.Message}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");
    }
}

async Task TestRefresh()
{
    Console.WriteLine("try refreshed");
    JwtDTO jwt = new JwtDTO() {JWT = jwtToken, Login = "Kolya1234", Refresh = refreshToken };
    var response = client.PostAsJsonAsync<JwtDTO>("https://localhost:7100/api/Authorization/refresh", jwt);
    var apiResponse = await response.Result.Content.ReadFromJsonAsync<ApiResponse<JwtDTO>>();
    if (apiResponse?.Success == true)
    {
        Console.WriteLine("refreshed");
        SetAuthToken(apiResponse.Data.JWT, apiResponse.Data.Refresh);
    }
}

void SetAuthToken(string token, string refresh)
{
    refreshToken = refresh;
    jwtToken = token;
    client.DefaultRequestHeaders.Authorization = new("Bearer", token);
}

Console.ReadLine();