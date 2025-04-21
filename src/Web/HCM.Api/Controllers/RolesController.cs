using HCM.BusinessLogic.Interfaces;
using HCM.BusinessLogic.Models.Roles;
using Microsoft.AspNetCore.Mvc;

namespace HCM.Api.Controllers;

[Route("api/roles")]
public class RolesController : ControllerBase
{
	private readonly IRoleService roleService;
	public RolesController(IRoleService roleService)
	{
		this.roleService = roleService;
	}

	[HttpGet("all")]
	public async Task<IEnumerable<RoleListResponseModel>> GetAll()
		=> await roleService.GetAll();
}
