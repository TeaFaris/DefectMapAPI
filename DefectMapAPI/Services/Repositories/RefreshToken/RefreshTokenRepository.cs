using DefectMapAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DefectMapAPI.Services.Repositories.RefreshToken
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        readonly ApplicationDbContext dbContext;
        public RefreshTokenRepository(ApplicationDbContext applicationDbContext)
        {
            this.dbContext = applicationDbContext;
        }

        public async Task AddAsync(Models.RefreshTokenModel.RefreshToken entity)
        {
            await dbContext.RefreshTokens.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<Models.RefreshTokenModel.RefreshToken> entities)
        {
            await dbContext.RefreshTokens.AddRangeAsync(entities);
        }

        public async Task<IEnumerable<Models.RefreshTokenModel.RefreshToken>> FindAsync(Expression<Func<Models.RefreshTokenModel.RefreshToken, bool>> predicate)
        {
            return await dbContext
                .RefreshTokens
                .Include(x => x.ApplicationUser)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Models.RefreshTokenModel.RefreshToken>> GetAllAsync()
        {
            return await dbContext
                .RefreshTokens
                .Include(x => x.ApplicationUser)
                .ToListAsync();
        }

        public async Task<Models.RefreshTokenModel.RefreshToken?> GetAsync(int id)
        {
            return await dbContext
                .RefreshTokens
                .Include(x => x.ApplicationUser)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task RemoveAsync(Models.RefreshTokenModel.RefreshToken entity)
        {
            dbContext.RefreshTokens.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task RemoveRangeAsync(IEnumerable<Models.RefreshTokenModel.RefreshToken> entities)
        {
            dbContext.RefreshTokens.RemoveRange(entities);
            await Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Models.RefreshTokenModel.RefreshToken entity)
        {
            dbContext.RefreshTokens.Update(entity);
            await Task.CompletedTask;
        }

        public async Task UpdateRangeAsync(IEnumerable<Models.RefreshTokenModel.RefreshToken> entities)
        {
            dbContext.RefreshTokens.UpdateRange(entities);
            await Task.CompletedTask;
        }
    }
}
