using DefectMapAPI.Models.Defect;
using DefectMapAPI.Models.File;
using DefectMapAPI.Models.RefreshTokenModel;
using DefectMapAPI.Models.UserModel;
using Microsoft.EntityFrameworkCore;

namespace DefectMapAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ApplicationUser> Users { get; init; }
        public DbSet<UploadedFile> UploadedFiles { get; init; }
        public DbSet<Defect> Defects { get; init; }

        public DbSet<RefreshToken> RefreshTokens { get; init; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
    }
}
