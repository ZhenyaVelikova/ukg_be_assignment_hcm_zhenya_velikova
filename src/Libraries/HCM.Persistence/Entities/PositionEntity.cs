using HCM.Core.Constants;
using HCM.Persistence.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HCM.Persistence.Entities;

[EntityTypeConfiguration(typeof(PositionEntity))]
public class PositionEntity : EntityBase<PositionEntity>
{
	public string Name { get; set; }

	public bool IsActive { get; set; }

	public virtual ICollection<PersonEntity> People { get; set; } = new HashSet<PersonEntity>();

	public override void Configure(EntityTypeBuilder<PositionEntity> entityTypeBuilder)
	{
		base.Configure(entityTypeBuilder);

		entityTypeBuilder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(ValueLengthConstants.Position.NameMaxLength);

		entityTypeBuilder.HasIndex(x => x.Name)
			.IsUnique();

		entityTypeBuilder.HasData(
				new List<PositionEntity>()
				{
					new PositionEntity
					{
						Id = new Guid("adb704cc-ee89-4f34-9b69-8f7a54d80e0d"),
						Name = "CEO",
						IsActive = true,
						CreatedAt = new DateTime(2025, 4, 17, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505),
						CreatedById = new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03")
					},
					new PositionEntity
					{
						Id = new Guid("ba3ca0a8-1560-4734-bc7b-7e6c3103d07d"),
						Name = "HR Manager",
						IsActive = true,
						CreatedAt = new DateTime(2025, 4, 17, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505),
						CreatedById = new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03")
					},
					new PositionEntity
					{
						Id = new Guid("44461ae7-0441-495d-bd78-1e67b5c9cc43"),
						Name = "Software Engineer",
						IsActive = true,
						CreatedAt = new DateTime(2025, 4, 17, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505),
						CreatedById = new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03")
					}
		});

	}
}
