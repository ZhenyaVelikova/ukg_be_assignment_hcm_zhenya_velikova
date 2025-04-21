using System.Linq.Expressions;

namespace HCM.Persistence.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
	Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
	Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
	Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

	Task<TEntity?> GetByIdAsync(
		Guid id,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
		CancellationToken cancellationToken = default);

	Task<IEnumerable<TEntity>> GetAllAsync(
	Expression<Func<TEntity, bool>>? predicate = null,
	Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
	CancellationToken cancellationToken = default);

	IQueryable<TEntity> GetQueryable();
}
