using System.ComponentModel.DataAnnotations;

namespace DefectMapAPI.Models.Shared.Requests
{
    public class RegisterUserRequest
    {
        [Required]
        [StringLength(32, MinimumLength = 5)]
        public string Username { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
