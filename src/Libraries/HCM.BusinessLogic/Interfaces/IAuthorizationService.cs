using HCM.BusinessLogic.Models.Authorization;

namespace HCM.BusinessLogic.Interfaces;

public interface IAuthorizationService
{
	Task<TokenResponseModel> SignIn(SignInRequestModel model);
	Task<TokenResponseModel> RefreshToken(RefreshTokenRequestModel model);
}
