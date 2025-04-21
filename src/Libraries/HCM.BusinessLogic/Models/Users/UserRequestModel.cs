using HCM.BusinessLogic.Models.People;

namespace HCM.BusinessLogic.Models.Users;

public class UserRequestModel
{
	public string UserName { get; set; } = default!;
	public string Password { get; set; } = default!;
	public bool IsActive { get; set; } 
	public Guid RoleId { get; set; } 
	public PersonRequestModel? Person { get; set; }
}
