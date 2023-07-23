using DefectMapAPI.Models.File;
using Microsoft.EntityFrameworkCore;

namespace DefectMapAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UploadedFile> UploadedFiles { get; init; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
    }
}
