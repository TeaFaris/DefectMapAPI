using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DefectMapAPI.Models.File
{
    public class UploadedFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string StoredFileName { get; set; }
        [Required]
        public string ContentType { get; set; }
    }
}
