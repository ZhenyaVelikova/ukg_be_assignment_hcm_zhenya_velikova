using HCM.BusinessLogic.Interfaces;
using HCM.BusinessLogic.Models.Departments;
using HCM.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCM.Api.Controllers;
[Route("api/departments")]
[ApiController]
[Authorize]
public class DepartmentssController : ControllerBase
{
	private readonly IDepartmentService departmentService;
	public DepartmentssController(IDepartmentService departmentService)
	{
		this.departmentService = departmentService;
	}
	[Authorize(Roles = RoleConstants.Manager + "," + RoleConstants.SystemAdmin)]
	[HttpGet, Route("all")]
	public async Task<IEnumerable<DepartmentListResponseModel>> GetAll()
	=> await departmentService.GetAll();

	[HttpGet]
	[Authorize(Roles = RoleConstants.Manager)]
	public async Task<IEnumerable<DepartmentListResponseModel>> Get([FromQuery] DepartmentFilterRequestModel model)
	=> await departmentService.GetPaged(model);


	[HttpGet("{id}")]
	public async Task<DepartmentResponseModel> Get([FromRoute] Guid id)
		=> await departmentService.Get(id);

	[HttpPost]
	[Authorize(Roles = RoleConstants.Manager + "," + RoleConstants.SystemAdmin)]
	public async Task Create([FromBody] DepartmentRequestModel model)
			=> await departmentService.CreateOrUpdate(null, model);

	[HttpPut("{id}")]
	[Authorize(Roles = RoleConstants.Manager + "," + RoleConstants.SystemAdmin)]
	public async Task Update([FromRoute] Guid id, [FromBody] DepartmentRequestModel model)
		=> await departmentService.CreateOrUpdate(id, model);

	[HttpDelete("{id}")]
	[Authorize(Roles = RoleConstants.SystemAdmin)]
	public async Task Delete([FromRoute] Guid id)
			=> await departmentService.Delete(id);
}