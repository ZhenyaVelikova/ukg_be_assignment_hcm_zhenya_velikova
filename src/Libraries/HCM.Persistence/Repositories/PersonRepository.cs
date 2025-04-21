using HCM.Persistence.Entities;
using HCM.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HCM.Persistence.Repositories;
public class PersonRepository : GenericRepository<PersonEntity>, IPersonRepository
{
	public PersonRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
	{
	}
	public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var count = await dbSet.AsNoTracking().Where(x => x.IsActive && x.Id == id).CountAsync();
		return count > 0;
	}


}
