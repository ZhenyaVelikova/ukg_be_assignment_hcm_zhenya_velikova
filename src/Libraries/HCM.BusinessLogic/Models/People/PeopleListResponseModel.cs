namespace HCM.BusinessLogic.Models.People;

public class PeopleListResponseModel
{
	public Guid PersonId { get; set; }
	public string FullName { get; set; } = default!;
	public string Email { get; set; } = default!;
	public DateTime StartDate { get; set; }
	public string? TerminationDate { get; set; }
	public string PositionName { get; set; } = default!;
	public string DepartmentName { get; set; } = default!;
}
