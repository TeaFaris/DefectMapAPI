using DefectMapAPI.Services.UserAuthenticationManagerService.Models;

namespace DefectMapAPI.Services.UserAuthenticationManagerService
{
    public interface IUserAuthenticationManager
    {
        Task<RegisterUserResult> Register(string username, string password);
        Task<LoginUserResult> ValidateCredentials(string username, string password);
    }
}
