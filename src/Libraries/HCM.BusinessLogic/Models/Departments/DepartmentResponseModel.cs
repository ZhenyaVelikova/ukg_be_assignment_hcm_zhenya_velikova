using HCM.BusinessLogic.Models.People;

namespace HCM.BusinessLogic.Models.Departments;

public class DepartmentResponseModel
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public bool IsActive { get; set; }
	public List<PersonResponseModel> People { get; set; } = new();
}
