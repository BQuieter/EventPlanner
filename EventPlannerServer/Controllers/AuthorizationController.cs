using Microsoft.AspNetCore.Mvc;
using EventPlannerServer.Services;
using EventPlannerLibrary;
using EventPlannerLibrary.ResponseDTOs;
using EventPlannerLibrary.RequestDTOs;

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
        public async Task<ActionResult<ApiResponse<JWTResponse>>> Login(AuthorizationUserRequest request)
        {
            if (!Validator<AuthorizationUserRequest>.IsValid(request))
                return BadRequest(request);
            return await Authorization(request);
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<JWTResponse>>> Registration(AuthorizationUserRequest request)
        {
            if (!Validator<AuthorizationUserRequest>.IsValid(request))
                return BadRequest(request);
            var result = authorizationService.Registration(request.Login, request.Password);
            if (!result.Item1)
                return BadRequest(ApiResponse<JWTResponse>.Fail(result.Item2));

            await loggerService.Log(request.Login, ActionTypes.Register, null, null);
            return await Authorization(request);
        }

        private async Task<ActionResult<ApiResponse<JWTResponse>>> Authorization(AuthorizationUserRequest request)
        {
            var result = authorizationService.Authorization(request.Login, request.Password);
            if (!result.Item1)
                return BadRequest(ApiResponse<JWTResponse>.Fail(result.Item2));

            loggerService.Log(request.Login, ActionTypes.Login, null, null);
            return Ok(ApiResponse<JWTResponse>.Ok(new() { Login = request.Login, JWT = result.Item2 }));
        }
    }
}
