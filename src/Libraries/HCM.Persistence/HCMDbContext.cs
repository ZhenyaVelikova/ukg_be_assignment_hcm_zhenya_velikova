using HCM.Core.Models;
using HCM.Persistence.Entities;
using HCM.Persistence.Entities.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HCM.Persistence;

public class HCMDbContext : DbContext
{
	private readonly IHttpContextAccessor httpContextAccessor;

	public HCMDbContext(DbContextOptions<HCMDbContext> options,
		IHttpContextAccessor httpContextAccessor)
		:base(options)
	{
		this.httpContextAccessor = httpContextAccessor;
	}

	#region DbSets
	public DbSet<UserEntity> Users { get; set; }

	public DbSet<RoleEntity> Roles { get; set; }

	public DbSet<UserRoleEntity> UserRoles { get; set; }

	public DbSet<DepartmentEntity> Departments { get; set; }

	public DbSet<PositionEntity> Positions { get; set; }

	public DbSet<PersonEntity> People { get; set; }

	public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
	#endregion

	protected override void OnModelCreating(ModelBuilder builder)
	{
		string[] tableWithDeleteCascade = new string[] { };

		var cascadeFKs = builder.Model.GetEntityTypes()
			.SelectMany(t => t.GetForeignKeys())
			.Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade
										 && !tableWithDeleteCascade.Contains(fk.DeclaringEntityType.Name.Split('.').Last()));

		foreach (var fk in cascadeFKs)
			fk.DeleteBehavior = DeleteBehavior.Restrict;

		base.OnModelCreating(builder);
	}

	public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
	{
		OnBeforeSaving();
		return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
	}

	private void OnBeforeSaving()
	{
		var entries = ChangeTracker.Entries();

		foreach (var entry in entries)
		{
			if (entry.Entity is ITrackable trackable)
			{
				var now = DateTime.UtcNow;
				var user = GetCurrentUser();

				switch (entry.State)
				{
					case EntityState.Modified:
						trackable.ModifiedAt = now;
						trackable.ModifiedById = user;
						break;

					case EntityState.Added:
						trackable.CreatedAt = now;
						trackable.CreatedById = user ?? trackable.CreatedById;
						break;

					case EntityState.Deleted:
						trackable.ModifiedAt = now;
						trackable.ModifiedById = user;
						break;
				}
			}
		}
	}

	private Guid? GetCurrentUser()
	{
		var ctx = httpContextAccessor?.HttpContext;
		var user = ctx?.User;
		var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (!Guid.TryParse(idClaim, out var id))
			return null;

		return id;
		
	}
}
