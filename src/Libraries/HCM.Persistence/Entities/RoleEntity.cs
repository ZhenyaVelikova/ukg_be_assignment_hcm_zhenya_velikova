using HCM.Core.Constants;
using HCM.Persistence.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HCM.Persistence.Entities;

[EntityTypeConfiguration(typeof(RoleEntity))]
public class RoleEntity : EntityBase<RoleEntity>
{
	public RoleEntity()
	{
		this.UserRoles = new HashSet<UserRoleEntity>();
		this.Users = new HashSet<UserEntity>();
	}
	public string? Name { get; set; }

	public string? DisplayName { get; set; }

	public virtual ICollection<UserRoleEntity> UserRoles { get; set; }

	public virtual ICollection<UserEntity> Users { get; set; }

	public override void Configure(EntityTypeBuilder<RoleEntity> entityTypeBuilder)
	{
		entityTypeBuilder.HasKey(x => x.Id);
		entityTypeBuilder.Property(x => x.Name).IsRequired().HasMaxLength(ValueLengthConstants.Role.NameMaxLength);
		entityTypeBuilder.Property(x => x.DisplayName).IsRequired().HasMaxLength(ValueLengthConstants.Role.DisplayNameMaxLength);
		entityTypeBuilder.HasIndex(x => x.Name);
		entityTypeBuilder.Ignore(x => x.CreatedBy);
		entityTypeBuilder.Ignore(x => x.ModifiedBy);
		entityTypeBuilder.HasData(
				new List<RoleEntity>()
				{
					new RoleEntity
					{
						Id = new Guid("8387a601-6341-4787-b779-f4efc2c8f33f"),
						Name = RoleConstants.SystemAdmin,
						DisplayName = "System Administrator",
					},
					new RoleEntity
					{
						Id = new Guid("3b9a2100-5e8e-4e57-a171-5af606db8b9b"),
						Name = RoleConstants.Manager,
						DisplayName = "Manager",
					},
					new RoleEntity
					{
						Id = new Guid("8cabb666-6f24-44ba-89ce-abd55e8782ed"),
						Name = RoleConstants.Employee,
						DisplayName = "Employee",
					}
		});
	}
}
