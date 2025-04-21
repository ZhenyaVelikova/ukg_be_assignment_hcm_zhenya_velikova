using HCM.Persistence.Entities;
using HCM.Persistence.Repositories;

namespace HCM.Persistence.Interfaces;
public interface IPersonRepository: IGenericRepository<PersonEntity>
{
	Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

}
