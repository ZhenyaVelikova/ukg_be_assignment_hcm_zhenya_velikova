using HCM.Persistence.Entities;

namespace HCM.Persistence.Interfaces;
public interface IDepartmentRepository : IGenericRepository<DepartmentEntity>
{
	Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
	Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
}
