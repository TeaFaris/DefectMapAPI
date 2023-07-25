using System.ComponentModel.DataAnnotations;

namespace DefectMapAPI.Models
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

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double HorizontalAccuracy { get; set; }
        public double VerticalAccuracy { get; set; }
    }
}
