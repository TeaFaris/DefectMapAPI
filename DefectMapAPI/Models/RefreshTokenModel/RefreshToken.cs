using DefectMapAPI.Models.UserModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DefectMapAPI.Models.RefreshTokenModel
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; init; }

        public int UserId { get; init; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; init; }

        public string Token { get; init; }
        public string JwtId { get; init; }

        public bool Used { get; set; }
        public bool Revoked { get; set; }

        public DateTime AddedDate { get; init; }
        public DateTime ExpiryDate { get; init; }
    }
}
