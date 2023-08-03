using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DefectMapAPI.Models.UserModel;

namespace DefectMapAPI.Models.Defect
{
    public class Defect
    {
        [Key]
        public int Id { get; init; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public List<Guid> PhotosIds { get; set; }

        public int OwnerId { get; init; }
        [ForeignKey(nameof(OwnerId))]
        public ApplicationUser Owner { get; init; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double HorizontalAccuracy { get; set; }
        public double VerticalAccuracy { get; set; }
    }
}
