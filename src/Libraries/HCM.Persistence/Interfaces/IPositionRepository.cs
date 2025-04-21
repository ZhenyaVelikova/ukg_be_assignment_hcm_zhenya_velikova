using HCM.Persistence.Entities;
using System.Linq.Expressions;

namespace HCM.Persistence.Interfaces;
public interface IPositionRepository : IGenericRepository<PositionEntity>
{
	Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
	Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
	IQueryable<PositionEntity> GetQueryable();
}
