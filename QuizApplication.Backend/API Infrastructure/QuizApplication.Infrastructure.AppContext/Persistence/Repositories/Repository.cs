using Microsoft.EntityFrameworkCore;
using QuizApplication.Contracts.Entities;
using QuizApplication.Contracts.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

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

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
        }

        public async Task<TEntity> GetAsync(Func<TEntity, bool> predicate)
        {
            return await Task.Run(() => _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefault(predicate));
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync().ConfigureAwait(false);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(TEntity entityToUpdate)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(entityToUpdate.Id); // To Avoid tracking error
            if (entity != null)
                _dbContext.Entry(entity).State = EntityState.Detached;
            _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await Task.Run(() => _dbContext.Set<TEntity>().Remove(entity)).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<TEntity> GetWithInclude<TEntity>(
            Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>,
            IIncludableQueryable<TEntity, object>> include = null)
            where TEntity : BaseEntity
        {
            IQueryable<TEntity> result = _dbContext.Set<TEntity>().Where(predicate);

            if (include != null)
            {
                result = include(result);
            }

            return result.AsQueryable();
        }

        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            await Task.Run(() => _dbContext.Set<TEntity>().RemoveRange(entities)).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(entities).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TEntity> Update(TEntity entityToUpdate)
        {
            _dbContext.Set<TEntity>().Update(entityToUpdate);
            await _dbContext.SaveChangesAsync();
            return entityToUpdate;
        }
    }
}
