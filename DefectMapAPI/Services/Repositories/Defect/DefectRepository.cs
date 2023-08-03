using DefectMapAPI.Data;
using DefectMapAPI.Services.Repositories.File;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DefectMapAPI.Services.Repositories.Defect
{
    public class DefectRepository : IDefectRepository
    {
        readonly ApplicationDbContext dbContext;
        public DefectRepository(
                ApplicationDbContext dbContext
            )
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(Models.Defect.Defect entity)
        {
            await dbContext.Defects.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<Models.Defect.Defect> entities)
        {
            await dbContext.Defects.AddRangeAsync(entities);
        }

        public async Task<IEnumerable<Models.Defect.Defect>> FindAsync(Expression<Func<Models.Defect.Defect, bool>> predicate)
        {
            return await dbContext
                .Defects
                .Include(x => x.Owner)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Models.Defect.Defect>> GetAllAsync()
        {
            return await dbContext
                .Defects
                .Include(x => x.Owner)
                .ToListAsync();
        }

        public async Task<Models.Defect.Defect?> GetAsync(int id)
        {
            return await dbContext
                .Defects
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task RemoveAsync(Models.Defect.Defect entity)
        {
            dbContext.Defects.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task RemoveRangeAsync(IEnumerable<Models.Defect.Defect> entities)
        {
            dbContext.Defects.RemoveRange(entities);
            await Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Models.Defect.Defect entity)
        {
            dbContext.Defects.Update(entity);
            await Task.CompletedTask;
        }

        public async Task UpdateRangeAsync(IEnumerable<Models.Defect.Defect> entities)
        {
            dbContext.Defects.UpdateRange(entities);
            await Task.CompletedTask;
        }
    }
}
