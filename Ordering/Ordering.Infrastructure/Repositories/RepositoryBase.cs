﻿using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain;
using Ordering.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace Ordering.Infrastructure.Repositories
{
	public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
	{
		protected readonly OrderContext _dbContext;

		public RepositoryBase(OrderContext dbContext)
		{
			this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}


		public async Task<IReadOnlyList<T>> GetAllAsync()
		{
			return await _dbContext.Set<T>().ToListAsync();
		}

		public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
		{
			return await _dbContext.Set<T>().Where(predicate).ToListAsync();
		}

		public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, string? includeString, bool disableTracking)
		{
			IQueryable<T> query = _dbContext.Set<T>();
			if (disableTracking) query = query.AsNoTracking();

			if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

			if (predicate != null) query = query.Where(predicate);

			if (orderBy != null)
				return await orderBy(query).ToListAsync();

			return await query.ToListAsync();
		}

		public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, List<Expression<Func<T, object>>>? includes, bool disableTracking)
		{
			IQueryable<T> query = _dbContext.Set<T>();
			if (disableTracking) query = query.AsNoTracking();

			if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));

			if (predicate != null) query = query.Where(predicate);

			if (orderBy != null)
				return await orderBy(query).ToListAsync();

			return await query.ToListAsync();
		}

		public virtual async Task<T> GetByIdAsync(int id)
		{
			return await _dbContext.Set<T>().FindAsync(id);
		}

		public async Task<T> AddAsync(T entity)
		{
			_dbContext.Set<T>().Add(entity);
			await _dbContext.SaveChangesAsync();
			return entity;
		}

		public async Task UpdateAsync(T entity)
		{
			_dbContext.Entry(entity).State = EntityState.Modified;
			await _dbContext.SaveChangesAsync();
		}

		public async Task DeleteAsync(T entity)
		{
			_dbContext.Set<T>().Remove(entity);
			await _dbContext.SaveChangesAsync();
		}
	}
}