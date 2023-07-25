using DefectMapAPI.Models.UserModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DefectMapAPI.Models.File;

namespace DefectMapAPI.Models.DefectModel
{
    public class DefectDTO
    {
        [Key]
        public int Id { get; init; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public List<UploadedFile> Photos { get; set; }

        [Required]
        public UserDTO Owner { get; init; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double HorizontalAccuracy { get; set; }
        public double VerticalAccuracy { get; set; }
    }
}
