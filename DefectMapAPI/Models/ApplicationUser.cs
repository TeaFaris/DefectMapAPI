using System.ComponentModel.DataAnnotations;

namespace DefectMapAPI.Models
{
    public class ApplicationUser
    {
        [Key]
        public int Id { get; init; }

        [Required]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }
    }
}
