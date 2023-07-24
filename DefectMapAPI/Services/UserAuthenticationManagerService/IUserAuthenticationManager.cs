using DefectMapAPI.Models;
using DefectMapAPI.Models.Shared.LoginUser;
using DefectMapAPI.Models.Shared.RegisterUser;
using DefectMapAPI.Models.Shared.Requests;

namespace DefectMapAPI.Services.UserAuthenticationManagerService
{
    public interface IUserAuthenticationManager
    {
        Task<RegisterUserResponse> Register(RegisterUserRequest request);
        Task<LoginUserResponse> ValidateCredentials(LoginUserRequest request);
    }
}
