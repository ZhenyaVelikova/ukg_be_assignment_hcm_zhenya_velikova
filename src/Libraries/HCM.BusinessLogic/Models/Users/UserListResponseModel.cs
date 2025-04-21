namespace HCM.BusinessLogic.Models.Users;

public class UserListResponseModel
{
	public Guid Id { get; set; }
	public string UserName { get; set; } = default!;
	public bool IsActive { get; set; }
	public Guid? RoleId { get; set; } 
	public string RoleName { get; set; } = default!; 
}