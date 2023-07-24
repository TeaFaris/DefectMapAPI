using System.ComponentModel.DataAnnotations;

namespace DefectMapAPI.Models.Shared.LoginUser
{
    public class LoginUserRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
