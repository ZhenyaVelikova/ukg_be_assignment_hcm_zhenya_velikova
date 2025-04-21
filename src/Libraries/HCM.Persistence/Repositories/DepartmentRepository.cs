using HCM.Persistence.Entities;
using HCM.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HCM.Persistence.Repositories;
public class DepartmentRepository : GenericRepository<DepartmentEntity>, IDepartmentRepository
{
	public DepartmentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
	{
	}

	public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var count = await dbSet.AsNoTracking().Where(x => x.IsActive && x.Id == id).CountAsync();
		return count > 0;
	}

	public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
	{
		var count = await dbSet.AsNoTracking().Where(x => x.IsActive && x.Name == name).CountAsync();
		return count > 0;
	}

}
