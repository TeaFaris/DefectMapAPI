using System.ComponentModel.DataAnnotations;

namespace DefectMapAPI.Models.UserModel
{
    public class ApplicationUser
    {
        [Key]
        public int Id { get; init; }

        [Required]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
