using HCM.BusinessLogic.Models.People;

namespace HCM.BusinessLogic.Models.Positions;

public class PositionResponseModel
{
	public Guid Id { get; set; }
	public string Name { get; set; } = null!;
	public bool IsActive { get; set; }
	public List<PersonResponseModel> People { get; set; } = new();
}
