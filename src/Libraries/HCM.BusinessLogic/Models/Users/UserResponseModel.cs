using HCM.BusinessLogic.Models.People;

namespace HCM.BusinessLogic.Models.Users;

public class UserResponseModel
{
	public Guid Id { get; set; }
	public string UserName { get; set; } = default!;
	public bool IsActive { get; set; }
	public Guid RoleId { get; set; } 
	public string RoleName { get; set; } = default!; 
	public PersonResponseModel? Person { get; set; }
}