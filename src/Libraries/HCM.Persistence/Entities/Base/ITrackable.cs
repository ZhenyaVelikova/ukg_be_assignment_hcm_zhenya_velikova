namespace HCM.Persistence.Entities.Base;

public interface ITrackable
{
	DateTime CreatedAt { get; set; }

	Guid CreatedById { get; set; }

	DateTime? ModifiedAt { get; set; }

	Guid? ModifiedById { get; set; }
}
