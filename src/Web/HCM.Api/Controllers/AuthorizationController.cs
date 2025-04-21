using HCM.BusinessLogic.Models.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = HCM.BusinessLogic.Interfaces.IAuthorizationService;

namespace HCM.Api.Controllers;

public class AuthorizationController : ControllerBase
{
	private readonly IAuthorizationService authorizationService;

	public AuthorizationController(IAuthorizationService authorizationService)
	{
		this.authorizationService = authorizationService;
	}

	[AllowAnonymous]
	[HttpPost("api/SignIn")]
	public async Task<TokenResponseModel> SignIn([FromBody] SignInRequestModel model)
	{
		return await authorizationService.SignIn(model);
	}

	[HttpPost("api/RefreshToken")]
	[AllowAnonymous]
	public async Task<TokenResponseModel> RefreshToken([FromBody] RefreshTokenRequestModel model)
			=> await authorizationService.RefreshToken(model);
}
