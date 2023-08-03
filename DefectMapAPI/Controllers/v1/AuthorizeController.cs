using DefectMapAPI.Models.ApiRequests;
using DefectMapAPI.Models.ApiResponses;
using DefectMapAPI.Services.JwtTokenGeneratorService;
using DefectMapAPI.Services.UserAuthenticationManagerService;
using Microsoft.AspNetCore.Mvc;

namespace DefectMapAPI.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthorizeController : ControllerBase
    {
        readonly IUserAuthenticationManager authenticationManager;
        readonly JwtTokenGenerator jwtTokenGenerator;
        public AuthorizeController(
                IUserAuthenticationManager authenticationManager,
                JwtTokenGenerator jwtTokenGenerator
            )
        {
            this.jwtTokenGenerator = jwtTokenGenerator;
            this.authenticationManager = authenticationManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var result = await authenticationManager.Register(registerRequest.Username, registerRequest.Password);

            if (!result.Successful)
            {
                return BadRequest(new RegisterResponse
                {
                    Successful = false,
                    Errors = result.Errors
                });
            }

            return Ok(new RegisterResponse
            {
                Successful = true
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var result = await authenticationManager.ValidateCredentials(loginRequest.Username, loginRequest.Password);

            if (!result.Successful)
            {
                return Unauthorized(new LoginResponse
                {
                    Successful = false
                });
            }

            var tokens = await jwtTokenGenerator.GenerateTokens(result.User!);

            SetCookiesRefreshToken(tokens.RefreshToken);

            return Ok(new LoginResponse
            {
                Successful = true,
                JwtToken = tokens.JwtToken,
                RefreshToken = tokens.RefreshToken
            });
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (refreshToken is null)
            {
                return Unauthorized("No 'refreshToken' provided in cookies.");
            }

            var generatedTokens = await jwtTokenGenerator.VerifyAndGenerateTokens(refreshToken);

            if (generatedTokens is null)
            {
                return Unauthorized("Invalid refresh token.");
            }

            SetCookiesRefreshToken(generatedTokens.RefreshToken);

            return Ok(generatedTokens);
        }

        private void SetCookiesRefreshToken(string refreshToken)
        {
            var cookiesOption = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMonths(1)
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookiesOption);
        }
    }
}
