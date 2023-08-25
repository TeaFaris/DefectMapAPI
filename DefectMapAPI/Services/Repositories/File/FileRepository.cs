using DefectMapAPI.Data;
using DefectMapAPI.Models.File;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DefectMapAPI.Services.Repositories.File
{
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationDbContext database;

        public FileRepository(ApplicationDbContext database)
        {
            this.database = database;
        }

        public async Task AddAsync(UploadedFile entity)
        {
            await database.UploadedFiles.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<UploadedFile> entities)
        {
            await database.UploadedFiles.AddRangeAsync(entities);
        }

        public async Task<IEnumerable<UploadedFile>> FindAsync(Expression<Func<UploadedFile, bool>> predicate)
        {
            await Task.CompletedTask;
            return database
                .UploadedFiles
                .Where(predicate)
                .ToList();
        }

        public async Task<UploadedFile?> GetAsync(Guid id)
        {
            return await database
                .UploadedFiles
                .FindAsync(id);
        }

        public async Task<IEnumerable<UploadedFile>> GetAllAsync()
        {
            return await database.UploadedFiles.ToListAsync();
        }

        public async Task RemoveAsync(UploadedFile entity)
        {
            database.UploadedFiles.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task RemoveRangeAsync(IEnumerable<UploadedFile> entities)
        {
            database.UploadedFiles.RemoveRange(entities);
            await Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await database.SaveChangesAsync();
        }

        public async Task UpdateAsync(UploadedFile entity)
        {
            database.Update(entity);
            await Task.CompletedTask;
        }

        public async Task UpdateRangeAsync(IEnumerable<UploadedFile> entities)
        {
            database.UpdateRange(entities);
            await Task.CompletedTask;
        }
    }
}
