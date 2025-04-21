using HCM.Core.Constants;
using HCM.Persistence.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HCM.Persistence.Entities;

[EntityTypeConfiguration(typeof(UserEntity))]
public class UserEntity : EntityBase<UserEntity>
{
	public UserEntity()
	{
		this.UserRoles = new HashSet<UserRoleEntity>();
		this.Roles = new HashSet<RoleEntity>();
	}

	public string? UserName { get; set; }

	public string? Password { get; set; }

	public bool IsActive { get; set; }

	public virtual ICollection<UserRoleEntity> UserRoles { get; set; }

	public virtual ICollection<RoleEntity> Roles { get; set; }

	public override void Configure(EntityTypeBuilder<UserEntity> entityTypeBuilder)
	{
		base.Configure(entityTypeBuilder);
		entityTypeBuilder.Property(x => x.UserName).IsRequired().HasMaxLength(ValueLengthConstants.User.UsernameMaxLength);
		entityTypeBuilder.Property(x => x.Password).IsRequired();

		entityTypeBuilder.HasIndex(x => x.UserName);

		entityTypeBuilder
				.HasMany(x => x.Roles)
				.WithMany(x => x.Users)
				.UsingEntity<UserRoleEntity>();

		entityTypeBuilder.HasData(
			new UserEntity
			{
				Id = new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03"),
				UserName = "admin",
				Password = "AFYNtbZhukrYLlPRPf/AZDgPfDwQhqOd1MKeISmNkSP5K2UaHLOUErSl/4iaoWZO4Q==", //Pass123!
				IsActive = true,
				CreatedAt = new DateTime(2025, 4, 17, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505),
				CreatedById = new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03")
			});

		entityTypeBuilder.HasData(new List<UserEntity>
		{

			new UserEntity
			{
				Id = new Guid("ef81dce2-ff4d-4abf-88a7-6ed24d13e4b6"),
				UserName = "john.smith",
				Password = "AFYNtbZhukrYLlPRPf/AZDgPfDwQhqOd1MKeISmNkSP5K2UaHLOUErSl/4iaoWZO4Q==", //Pass123!
				IsActive = true,
				CreatedAt = new DateTime(2025, 4, 17, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505),
				CreatedById = new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03")
			},
			new UserEntity
			{
				Id = new Guid("4bd3c8c1-b7fd-4f91-a629-71e7182ac88a"),
				UserName = "taylor.jones",
				Password = "AFYNtbZhukrYLlPRPf/AZDgPfDwQhqOd1MKeISmNkSP5K2UaHLOUErSl/4iaoWZO4Q==", //Pass123!
				IsActive = true,
				CreatedAt = new DateTime(2025, 4, 17, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505),
				CreatedById = new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03")
			},
			new UserEntity
			{
				Id = new Guid("a97a0432-6a4f-4f88-8997-af7cc7f806c2"),
				UserName = "stuart.brooks",
				Password = "AFYNtbZhukrYLlPRPf/AZDgPfDwQhqOd1MKeISmNkSP5K2UaHLOUErSl/4iaoWZO4Q==", //Pass123!
				IsActive = true,
				CreatedAt = new DateTime(2025, 4, 17, 7, 22, 31, 21, DateTimeKind.Utc).AddTicks(4505),
				CreatedById = new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03")
			}
		});
	}
}
