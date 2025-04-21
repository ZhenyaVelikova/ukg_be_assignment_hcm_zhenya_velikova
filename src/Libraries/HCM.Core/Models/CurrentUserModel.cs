namespace HCM.Core.Models;

public class CurrentUserModel
{
	public CurrentUserModel()
	{
		this.Roles = new HashSet<CurrentUserRoleModel>();
	}

	public Guid Id { get; set; }

	public string Username { get; set; }

	public IEnumerable<CurrentUserRoleModel> Roles { get; set; }
}