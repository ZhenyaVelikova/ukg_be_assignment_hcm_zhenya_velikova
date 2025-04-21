using HCM.Core.Constants;
using HCM.Core.Enums;
using HCM.Persistence.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HCM.Persistence.Entities;

[EntityTypeConfiguration(typeof(PersonEntity))]
public class PersonEntity : EntityBase<PersonEntity>
{
	public Guid UserId { get; set; }
	public virtual UserEntity User { get; set; }

	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string FullName => $"{FirstName} {LastName}";

	public string? Email { get; set; }

	public Guid DepartmentId { get; set; }
	public virtual DepartmentEntity Department { get; set; }

	public Guid PositionId { get; set; }
	public virtual PositionEntity Position { get; set; }

	public DateTime StartDate { get; set; }
	public DateTime? TerminationDate { get; set; }

	public PersonType PersonType { get; set; }

	public bool IsActive { get; set; }

	public Guid? ReportsToId { get; set; }
	public virtual PersonEntity? ReportsTo { get; set; }

	public virtual ICollection<PersonEntity> DirectReports { get; set; } = new HashSet<PersonEntity>();

	public override void Configure(EntityTypeBuilder<PersonEntity> entityTypeBuilder)
	{
		base.Configure(entityTypeBuilder);

		entityTypeBuilder.Property(x => x.FirstName).IsRequired().HasMaxLength(ValueLengthConstants.Person.FirstNameMaxLength);
		entityTypeBuilder.Property(x => x.LastName).IsRequired().HasMaxLength(ValueLengthConstants.Person.LastNameMaxLength);
		entityTypeBuilder.Property(x => x.Email).IsRequired().HasMaxLength(ValueLengthConstants.Person.EmailMaxLength);
		entityTypeBuilder.Property(x => x.StartDate).IsRequired();

		entityTypeBuilder.Ignore(x => x.FullName);

		entityTypeBuilder.HasIndex(x => x.Email).IsUnique();

		entityTypeBuilder.HasOne(x => x.User)
			.WithOne()
			.HasForeignKey<PersonEntity>(x => x.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		entityTypeBuilder.HasOne(x => x.Department)
			.WithMany()
			.HasForeignKey(x => x.DepartmentId)
			.OnDelete(DeleteBehavior.Restrict);

		entityTypeBuilder.HasOne(x => x.Position)
			.WithMany()
			.HasForeignKey(x => x.PositionId)
			.OnDelete(DeleteBehavior.Restrict);

		entityTypeBuilder.HasOne(x => x.ReportsTo)
			.WithMany(x => x.DirectReports)
			.HasForeignKey(x => x.ReportsToId)
			.OnDelete(DeleteBehavior.Restrict);

		entityTypeBuilder.HasOne(x => x.ReportsTo)
			.WithMany(x => x.DirectReports)
			.HasForeignKey(x => x.ReportsToId)
			.OnDelete(DeleteBehavior.Restrict);

		entityTypeBuilder.HasData(new List<PersonEntity> {

			new PersonEntity
			{
				Id = new Guid("79b22586-b339-445e-8c2b-9aa4381f0e01"),
				UserId = new Guid("ef81dce2-ff4d-4abf-88a7-6ed24d13e4b6"),
				FirstName = "John",
				LastName = "Smith",
				IsActive = true,
				Email = "john.smith@domain.com",
				PositionId = new Guid("adb704cc-ee89-4f34-9b69-8f7a54d80e0d"),
				DepartmentId = new Guid("18220425-685f-4105-ab7b-7aa8ec50c5df"),
				StartDate = new DateTime(2023, 5, 18, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505),
				CreatedById = new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03")
			},
			new PersonEntity
			{
				Id = new Guid("9ee79e7b-7c87-4f84-af10-85f504ad348c"),
				UserId = new Guid("4bd3c8c1-b7fd-4f91-a629-71e7182ac88a"),
				FirstName = "Taylor",
				LastName = "Jones",
				IsActive = true,
				Email = "taylor.jones@domain.com",
				PositionId = new Guid("ba3ca0a8-1560-4734-bc7b-7e6c3103d07d"),
				DepartmentId = new Guid("efc48980-042d-4e52-98d9-daa6c1deeaa7"),
				StartDate = new DateTime(2023, 6, 18, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505),
				ReportsToId = new Guid("79b22586-b339-445e-8c2b-9aa4381f0e01"),
				CreatedById = new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03")
			},
			new PersonEntity
			{
				Id = new Guid("fe337be1-1e81-4e54-9d0b-a45cd6b09527"),
				UserId = new Guid("a97a0432-6a4f-4f88-8997-af7cc7f806c2"),
				FirstName = "Stuart",
				LastName = "Brooks",
				IsActive = true,
				Email = "stuart.brooks@domain.com",
				PositionId = new Guid("44461ae7-0441-495d-bd78-1e67b5c9cc43"),
				DepartmentId = new Guid("aa6228b7-3507-42ce-a479-7335d9bb396b"),
				StartDate = new DateTime(2023, 7, 18, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505),
				ReportsToId = new Guid("79b22586-b339-445e-8c2b-9aa4381f0e01"),
				CreatedById = new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03")
			}
		});
	}


}
