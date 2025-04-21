using HCM.Persistence.Entities;
using HCM.Persistence.Interfaces;

namespace HCM.Persistence.Repositories;

public class RoleRepository : GenericRepository<RoleEntity>, IRoleRepository
{
	public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
	{
	}
}

