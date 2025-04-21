namespace HCM.BusinessLogic.Models.Users;

public class UserFilterRequestModel
{
	public string? SearchTerm { get; set; }
	public bool? IsActive { get; set; }
	public int Page { get; set; } = 1;
	public int PageSize { get; set; } = 10;
}
