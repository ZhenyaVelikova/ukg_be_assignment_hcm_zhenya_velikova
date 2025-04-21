using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HCM.Persistence.Entities.Base;

public abstract class EntityBase<TEntity> : IEntityBase, IEntityTypeConfiguration<TEntity>, ITrackable
	where TEntity : EntityBase<TEntity>
{
	[Key]
	public Guid Id { get; set; }

	public virtual bool IsNew()
	{
		_ = Id;
		return Id == Guid.Empty;
	}
	public DateTime CreatedAt { get; set; }

	public Guid CreatedById { get; set; }

	public virtual UserEntity CreatedBy { get; set; }

	public DateTime? ModifiedAt { get; set; }

	public Guid? ModifiedById { get; set; }

	public virtual UserEntity ModifiedBy { get; set; }
	
	public virtual void Configure(EntityTypeBuilder<TEntity> entityTypeBuilder)
	{
		entityTypeBuilder.HasKey(x => x.Id);
		entityTypeBuilder.HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById).IsRequired().OnDelete(DeleteBehavior.Restrict);
		entityTypeBuilder.HasOne(x => x.ModifiedBy).WithMany().HasForeignKey(x => x.ModifiedById).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
	}
}
