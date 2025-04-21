using HCM.Core.Enums;

namespace HCM.BusinessLogic.Models.People;

public class PersonRequestModel
{
	public string FirstName { get; set; } = default!;
	public string LastName { get; set; } = default!;
	public string Email { get; set; } = default!;
	public Guid DepartmentId { get; set; }
	public Guid PositionId { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime? TerminationDate { get; set; }
	public PersonType PersonType { get; set; }
	public Guid? ReportsToId { get; set; }
}
