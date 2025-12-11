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

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<JWTResponse>>> Authorization(AuthorizationUserRequest request)
        {
            if (!Validator<AuthorizationUserRequest>.IsValid(request))
                return BadRequest(request);
            var result = authorizationService.Authorization(request.Login, request.Password);
            if (!result.Item1)
                return BadRequest(ApiResponse<JWTResponse>.Fail(result.Item2));
            return Ok(ApiResponse<JWTResponse>.Ok(new() { Login = request.Login, JWT = result.Item2}));
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<JWTResponse>>> Registration(AuthorizationUserRequest request)
        {
            if (!Validator<AuthorizationUserRequest>.IsValid(request))
                return BadRequest(request);
            var result = authorizationService.Registration(request.Login, request.Password);
            if (!result.Item1)
                return BadRequest(ApiResponse<JWTResponse>.Fail(result.Item2));
            return Ok(ApiResponse<JWTResponse>.Ok(new() { Login = request.Login, JWT = result.Item2 }));
        }
    }
}
