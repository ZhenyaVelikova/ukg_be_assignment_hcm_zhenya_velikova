using HCM.BusinessLogic.Interfaces;
using HCM.BusinessLogic.Models.Roles;
using HCM.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HCM.BusinessLogic.Services;
public class RoleService : IRoleService
{
	private readonly IRoleRepository roleRepository;
	public RoleService(IRoleRepository roleRepository)
	{
		this.roleRepository = roleRepository;
	}

	public async Task<IEnumerable<RoleListResponseModel>> GetAll()
	{
		var roles = await roleRepository.GetQueryable()
			.OrderBy(x => x.Name)
			.ToListAsync();
		return roles.Select(x => new RoleListResponseModel
		{
			Id = x.Id,
			Name = x.Name
		});
	}
}
