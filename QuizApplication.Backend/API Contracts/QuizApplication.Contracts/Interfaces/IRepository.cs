﻿using QuizApplication.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;

namespace QuizApplication.Contracts.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(int id);

        Task<TEntity> GetAsync(Func<TEntity, bool> predicate);

        Task<IReadOnlyList<TEntity>> GetAllAsync();

        Task<TEntity> AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        Task UpdateAsync(TEntity entity);
        Task<TEntity> Update(TEntity entityToUpdate);

        Task DeleteAsync(TEntity entity);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities);

        IQueryable<TEntity> GetWithInclude<TEntity>(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
            where TEntity : BaseEntity;
    }
}
