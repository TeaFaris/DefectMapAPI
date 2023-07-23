using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DefectMapAPI.Models.File
{
    public class UploadedFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }
        [Required]
        public string FileName { get; init; }
        [Required]
        public string StoredFileName { get; init; }
        [Required]
        public string ContentType { get; init; }
    }
}
