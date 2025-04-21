namespace HCM.Persistence.Entities.Base;

public interface IEntityBase
{
	Guid Id { get; set; }

	bool IsNew();
}