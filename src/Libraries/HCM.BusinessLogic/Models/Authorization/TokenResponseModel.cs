namespace HCM.BusinessLogic.Models.Authorization;

public class TokenResponseModel
{
	public string AccessToken { get; set; }

	public string RefreshToken { get; set; }

	public string Type { get; set; }
}
