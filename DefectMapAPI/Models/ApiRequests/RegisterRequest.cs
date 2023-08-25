using System.ComponentModel.DataAnnotations;

namespace DefectMapAPI.Models.ApiRequests
{
    public class RegisterRequest
    {
        [StringLength(32, MinimumLength = 5)]
        public string Username { get; init; }

        [StringLength(32, MinimumLength = 6)]
        public string Password { get; init; }
    }
}
