using DefectMapAPI.Models;

namespace DefectMapAPI.Services.UserAuthenticationManagerService.Models
{
    public class RegisterUserResult
    {
        public bool Successful { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
