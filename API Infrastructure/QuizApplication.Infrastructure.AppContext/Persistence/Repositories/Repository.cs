using Microsoft.EntityFrameworkCore;
using QuizApplication.Contracts.Entities;
using QuizApplication.Contracts.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizApplication.Infrastructure.AppContext.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly AppDbContext _dbContext;
        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity).ConfigureAwait(false);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(entities).ConfigureAwait(false);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await Task.Run(() => _dbContext.Set<TEntity>().Remove(entity)).ConfigureAwait(false);
        }

        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            await Task.Run(() => _dbContext.Set<TEntity>().RemoveRange(entities)).ConfigureAwait(false);
        }
        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync().ConfigureAwait(false);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
        }
    }
}
