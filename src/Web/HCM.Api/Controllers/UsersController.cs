using HCM.BusinessLogic.Interfaces;
using HCM.BusinessLogic.Models.People;
using HCM.BusinessLogic.Models.Users;
using HCM.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCM.Api.Controllers;

[Route("api/users")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
	private readonly IUserService userService;

	public UsersController(IUserService userService)
	{
		this.userService = userService;
	}

	[HttpGet]
	[Authorize(Roles = RoleConstants.SystemAdmin)]
	public async Task<IEnumerable<UserListResponseModel>> Get([FromQuery] UserFilterRequestModel model)
		=> await userService.GetPaged(model);

	[HttpGet("people")]
	[Authorize(Roles = RoleConstants.Manager + "," + RoleConstants.SystemAdmin + "," + RoleConstants.Employee)]
	public async Task<IEnumerable<PeopleListResponseModel>> GetPeople([FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken ct = default)
		=> await userService.GetPeople(page, pageSize, ct);

	[HttpGet("{id}")]
	public async Task<UserResponseModel> Get([FromRoute] Guid id)
		=> await userService.Get(id);


	[HttpPost]
	[Authorize(Roles = RoleConstants.Manager + "," + RoleConstants.SystemAdmin)]
	public async Task Create([FromBody] UserRequestModel model)
			=> await userService.CreateOrUpdate(null, model);

	[HttpPut("{id}")]
	[Authorize(Roles = RoleConstants.Manager + "," + RoleConstants.SystemAdmin)]
	public async Task Update([FromRoute] Guid id, [FromBody] UserRequestModel model)
		=> await userService.CreateOrUpdate(id, model);

	[HttpDelete("{id}")]
	[Authorize(Roles = RoleConstants.SystemAdmin)]
	public async Task Delete([FromRoute] Guid id)
			=> await userService.Delete(id);

	[HttpPost("{id}/change-password")]
	[Authorize]
	public async Task ChangePassword([FromRoute] Guid id, [FromBody] ChangePasswordModel model)
	=> await userService.ChangePassword(id, model);
}
