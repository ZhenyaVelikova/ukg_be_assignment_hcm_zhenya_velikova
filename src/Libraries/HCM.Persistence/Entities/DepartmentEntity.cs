using HCM.Core.Constants;
using HCM.Persistence.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HCM.Persistence.Entities;

[EntityTypeConfiguration(typeof(DepartmentEntity))]
public class DepartmentEntity : EntityBase<DepartmentEntity>
{
	public string Name { get; set; }

	public bool IsActive { get; set; }

	public virtual ICollection<PersonEntity> People { get; set; } = new HashSet<PersonEntity>();

	public override void Configure(EntityTypeBuilder<DepartmentEntity> entityTypeBuilder)
	{
		base.Configure(entityTypeBuilder);

		entityTypeBuilder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(ValueLengthConstants.Department.NameMaxLength);

		entityTypeBuilder.HasIndex(x => x.Name)
			.IsUnique();

		entityTypeBuilder.HasData(
				new List<DepartmentEntity>()
				{
					new DepartmentEntity
					{
						Id = new Guid("18220425-685f-4105-ab7b-7aa8ec50c5df"),
						Name = "Management",
						IsActive = true,
						CreatedAt = new DateTime(2025, 4, 17, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505),
						CreatedById = new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03")
					},
					new DepartmentEntity
					{
						Id = new Guid("efc48980-042d-4e52-98d9-daa6c1deeaa7"),
						Name = "HR",
						IsActive = true,
						CreatedAt = new DateTime(2025, 4, 17, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505),
						CreatedById = new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03")
					},
					new DepartmentEntity
					{
						Id = new Guid("aa6228b7-3507-42ce-a479-7335d9bb396b"),
						Name = "Software",
						IsActive = true,
						CreatedAt = new DateTime(2025, 4, 17, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505),
						CreatedById = new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03")
					}
		});
	}
}
