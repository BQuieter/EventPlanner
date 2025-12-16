using Microsoft.AspNetCore.Mvc;
using EventPlannerServer.Services;
using EventPlannerLibrary;
using EventPlannerLibrary.RequestDTOs;
using EventPlannerLibrary.SharedDTOs;

namespace EventPlannerServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController: ControllerBase
    {
        private IAuthorizationService authorizationService;
        private ILoggerService loggerService;

        public AuthorizationController(IAuthorizationService authorizationService, ILoggerService loggerService)
        {
            this.authorizationService = authorizationService;
            this.loggerService = loggerService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<JwtDTO>>> Login(AuthorizationUserRequest requestData)
        {
            if (!Validator<AuthorizationUserRequest>.IsValid(requestData))
                return BadRequest(ApiResponse<JwtDTO>.Fail("Неккоректные данные", "400"));
            return await Authorization(requestData);
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<JwtDTO>>> Registration(AuthorizationUserRequest requestData)
        {
            if (!Validator<AuthorizationUserRequest>.IsValid(requestData))
                return BadRequest(requestData);
            var result = authorizationService.Registration(requestData.Login, requestData.Password);
            if (result.Item1 is not null)
                return BadRequest(ApiResponse<JwtDTO>.Fail(result.Item1.Message, result.Item1.ErrorCode));

            await loggerService.Log(requestData.Login, ActionTypes.Register, null, null);
            return await Authorization(requestData);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<ApiResponse<JwtDTO>>> Refresh(JwtDTO requestData)
        {
            if (!Validator<JwtDTO>.IsValid(requestData))
                return BadRequest(requestData);
            var result = authorizationService.RefreshJWTToken(requestData.Login, requestData.JWT, requestData.Refresh!);
            if (result.Item1 is not null)
                return BadRequest(ApiResponse<JwtDTO>.Fail(result.Item1.Message, result.Item1.ErrorCode));
            return Ok(ApiResponse<JwtDTO>.Ok(new() { Login = requestData.Login, JWT = result.Item2, Refresh = result.Item3 }));
        }

        private async Task<ActionResult<ApiResponse<JwtDTO>>> Authorization(AuthorizationUserRequest requestData)
        {
            var result = authorizationService.Authorization(requestData.Login, requestData.Password);
            if (result.Item1 is not null)
                return BadRequest(ApiResponse<JwtDTO>.Fail(result.Item1.Message, result.Item1.ErrorCode));

            await loggerService.Log(requestData.Login, ActionTypes.Login, null, null);
            return Ok(ApiResponse<JwtDTO>.Ok(new() { Login = requestData.Login, JWT = result.Item2, Refresh = result.Item3 }));
        }
    }
}
