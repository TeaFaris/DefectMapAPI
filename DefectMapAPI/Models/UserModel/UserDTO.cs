using System.ComponentModel.DataAnnotations;

namespace DefectMapAPI.Models.UserModel
{
    public class UserDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
