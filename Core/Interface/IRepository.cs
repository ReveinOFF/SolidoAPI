﻿using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IRepository<TEntity> where TEntity : class, IBaseEntity
    {
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                    string includeProperties = "");

        Task<TEntity> GetByIdAsync<TKey>(TKey id);
        Task InsertAsync(TEntity entity);
        Task DeleteAsync<TKey>(TKey id);
        Task DeleteAsync(TEntity entityToDelete);
        Task UpdateAsync(TEntity entityToUpdate);
    }
    public interface IUnitOfWork
    {
        IRepository<Roof> RoofRepository { get; }
        IRepository<Contact> ContactRepository { get; }
        IRepository<Main> MainsRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
