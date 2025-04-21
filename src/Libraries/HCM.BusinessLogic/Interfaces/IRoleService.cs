using HCM.BusinessLogic.Models.Roles;

namespace HCM.BusinessLogic.Interfaces;
public interface IRoleService
{
	Task<IEnumerable<RoleListResponseModel>> GetAll();
}
