using HCM.Persistence.Entities.Base;
using HCM.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace HCM.Persistence.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntityBase
{
	protected readonly DbSet<TEntity> dbSet;
	protected readonly IUnitOfWork unitOfWork;
	protected readonly DbContext context;

	public GenericRepository(IUnitOfWork unitOfWork)
	{
		this.unitOfWork = unitOfWork;
		this.context = unitOfWork.Context;
		this.dbSet = context.Set<TEntity>();
	}

	public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		return (await dbSet.AddAsync(entity, cancellationToken).ConfigureAwait(false)).Entity;
	}

	public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		dbSet.Update(entity);
		return Task.CompletedTask;
	}

	public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		dbSet.Remove(entity);
		return Task.CompletedTask;
	}

	public async Task<TEntity?> GetByIdAsync(
		Guid id,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
		CancellationToken cancellationToken = default)
	{
		var query = dbSet.AsNoTracking();

		if (include != null)
			query = include(query);

		return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
			.ConfigureAwait(false);
	}


	public async Task<IEnumerable<TEntity>> GetAllAsync(
	Expression<Func<TEntity, bool>>? predicate = null,
	Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
	CancellationToken cancellationToken = default)
	{
		IQueryable<TEntity> query = dbSet.AsNoTracking();

		if (predicate != null)
		{
			query = query.Where(predicate);
		}

		if (include != null)
		{
			query = include(query);
		}

		return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
	}



	public IQueryable<TEntity> GetQueryable()
	{
		return dbSet.AsNoTracking();
	}
}
