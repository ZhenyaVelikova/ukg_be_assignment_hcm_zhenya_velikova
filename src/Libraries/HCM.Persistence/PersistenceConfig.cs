using HCM.Core.Configurations;
using HCM.Persistence.Interfaces;
using HCM.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HCM.Persistence;

public static class PersistenceConfig
{
	public static void ConfigurePersistence(this IServiceCollection services, ConnectionStrings connectionStrings)
	{
		if (connectionStrings?.SQLConnection == null)
			throw new ArgumentNullException(nameof(connectionStrings.SQLConnection));

		services.AddDbContext<HCMDbContext>(options =>
			options
				.UseSqlServer(connectionStrings.SQLConnection, sqlOptions =>
			{
				sqlOptions.MigrationsAssembly(typeof(PersistenceConfig).GetTypeInfo().Assembly.GetName().Name);
				sqlOptions.CommandTimeout((int)TimeSpan.FromSeconds(3).TotalSeconds);
			}),
			contextLifetime: ServiceLifetime.Transient
		);

		services.AddScoped<DbContext, HCMDbContext>();

		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IRoleRepository, RoleRepository>();
		services.AddScoped<IPersonRepository, PersonRepository>();
		services.AddScoped<IPositionRepository, PositionRepository>();
		services.AddScoped<IDepartmentRepository, DepartmentRepository>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

	}
}
