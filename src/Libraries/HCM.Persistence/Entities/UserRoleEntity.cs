using HCM.Persistence.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HCM.Persistence.Entities;

[EntityTypeConfiguration(typeof(UserRoleEntity))]
public class UserRoleEntity : IEntityTypeConfiguration<UserRoleEntity>
{
	public Guid UserId { get; set; }

	public virtual UserEntity User { get; set; }

	public Guid RoleId { get; set; }
	
	public virtual RoleEntity Role { get; set; }

	public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
	{
		builder.HasKey(x => new { x.UserId, x.RoleId });

		builder.HasData(
			new List<UserRoleEntity>
			{
					new UserRoleEntity
					{
						UserId = new Guid("7776f64f-8f22-47f9-8560-f24422fe4c03"),
						RoleId = new Guid("8387a601-6341-4787-b779-f4efc2c8f33f")
					},
					new UserRoleEntity
					{
						UserId = new Guid("ef81dce2-ff4d-4abf-88a7-6ed24d13e4b6"),
						RoleId = new Guid("8387a601-6341-4787-b779-f4efc2c8f33f")
					},
					new UserRoleEntity
					{
						UserId = new Guid("4bd3c8c1-b7fd-4f91-a629-71e7182ac88a"),
						RoleId = new Guid("3b9a2100-5e8e-4e57-a171-5af606db8b9b")
					},
					new UserRoleEntity
					{
						UserId = new Guid("a97a0432-6a4f-4f88-8997-af7cc7f806c2"),
						RoleId = new Guid("8cabb666-6f24-44ba-89ce-abd55e8782ed")
					},

			});
	}
}
