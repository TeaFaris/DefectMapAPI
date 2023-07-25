﻿using DefectMapAPI.Data;
using DefectMapAPI.Services.Repositories.File;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DefectMapAPI.Services.Repositories.Defect
{
    public class DefectRepository : IDefectRepository
    {
        readonly ApplicationDbContext dbContext;
        readonly IFileRepository fileRepository;
        public DefectRepository(
                ApplicationDbContext dbContext
            )
        {
            this.fileRepository = fileRepository;
            this.dbContext = dbContext;
        }

        public async Task AddAsync(Models.Defect entity)
        {
            await dbContext.Defects.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<Models.Defect> entities)
        {
            await dbContext.Defects.AddRangeAsync(entities);
        }

        public async Task<IEnumerable<Models.Defect>> FindAsync(Expression<Func<Models.Defect, bool>> predicate)
        {
            return await dbContext
                .Defects
                .Include(x => x.Owner)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Models.Defect>> GetAllAsync()
        {
            return await dbContext.Defects.ToListAsync();
        }

        public async Task<Models.Defect?> GetAsync(int id)
        {
            return await dbContext.Defects.FindAsync(id);
        }

        public async Task RemoveAsync(Models.Defect entity)
        {
            dbContext.Defects.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task RemoveRangeAsync(IEnumerable<Models.Defect> entities)
        {
            dbContext.Defects.RemoveRange(entities);
            await Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Models.Defect entity)
        {
            dbContext.Defects.Update(entity);
            await Task.CompletedTask;
        }

        public async Task UpdateRangeAsync(IEnumerable<Models.Defect> entities)
        {
            dbContext.Defects.UpdateRange(entities);
            await Task.CompletedTask;
        }
    }
}