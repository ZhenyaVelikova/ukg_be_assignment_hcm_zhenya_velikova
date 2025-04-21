using HCM.BusinessLogic.Interfaces;
using HCM.BusinessLogic.Models.Departments;
using HCM.BusinessLogic.Models.Positions;
using HCM.BusinessLogic.Services;
using HCM.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCM.Api.Controllers;

[Route("api/positions")]
[ApiController]
[Authorize]
public class PositionsController : ControllerBase
{
	private readonly IPositionService positionService;
	public PositionsController(IPositionService positionService)
	{
		this.positionService = positionService;
	}
	[Authorize(Roles = RoleConstants.Manager + "," + RoleConstants.SystemAdmin)]
	[HttpGet("all")]
	public async Task<IEnumerable<PositionListResponseModel>> GetAll()
	=> await positionService.GetAll();

	[HttpGet]
	[Authorize(Roles = RoleConstants.Manager)]
	public async Task<IEnumerable<PositionListResponseModel>> Get([FromQuery] PositionFilterRequestModel model)
	=> await positionService.GetPaged(model);


	[HttpGet("{id}")]
	public async Task<PositionResponseModel> Get([FromRoute] Guid id)
		=> await positionService.Get(id);

	[HttpPost]
	[Authorize(Roles = RoleConstants.Manager + "," + RoleConstants.SystemAdmin)]
	public async Task Create([FromBody] PositionRequestModel model)
			=> await positionService.CreateOrUpdate(null, model);

	[HttpPut("{id}")]
	[Authorize(Roles = RoleConstants.Manager + "," + RoleConstants.SystemAdmin)]
	public async Task Update([FromRoute] Guid id, [FromBody] PositionRequestModel model)
		=> await positionService.CreateOrUpdate(id, model);

	[HttpDelete("{id}")]
	[Authorize(Roles = RoleConstants.SystemAdmin)]
	public async Task Delete([FromRoute] Guid id)
			=> await positionService.Delete(id);
}
