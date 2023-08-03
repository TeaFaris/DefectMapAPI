using DefectMapAPI.Data;
using DefectMapAPI.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DefectMapAPI.Services.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(ApplicationUser entity)
        {
            await dbContext
                .Users
                .AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<ApplicationUser> entities)
        {
            await dbContext
                .Users
                .AddRangeAsync(entities);
        }

        public async Task<IEnumerable<ApplicationUser>> FindAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await dbContext
                .Users
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await dbContext
                .Users
                .ToListAsync();
        }

        public async Task<ApplicationUser?> GetAsync(int id)
        {
            return await dbContext
                .Users
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task RemoveAsync(ApplicationUser entity)
        {
            dbContext.Users.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task RemoveRangeAsync(IEnumerable<ApplicationUser> entities)
        {
            dbContext.Users.RemoveRange(entities);
            await Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(ApplicationUser entity)
        {
            dbContext.Users.Update(entity);
            await Task.CompletedTask;
        }

        public async Task UpdateRangeAsync(IEnumerable<ApplicationUser> entities)
        {
            dbContext.Users.UpdateRange(entities);
            await Task.CompletedTask;
        }
    }
}
