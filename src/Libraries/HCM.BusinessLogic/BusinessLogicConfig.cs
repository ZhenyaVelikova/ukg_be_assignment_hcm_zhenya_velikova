using HCM.BusinessLogic.Interfaces;
using HCM.BusinessLogic.Services;
using HCM.Core.Configurations;
using HCM.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace HCM.BusinessLogic;
public static class BusinessLogicConfig
{
	public static void ConfigureBusinessLogic(this IServiceCollection services, ConnectionStrings connectionStrings)
	{

		services.ConfigurePersistence(connectionStrings);

		services.AddScoped<IAuthorizationService, AuthorizationService>();
		services.AddScoped<IUserService, UserService>();
		services.AddScoped<IDepartmentService, DepartmentService>();
		services.AddScoped<IPositionService, PositionService>();
		services.AddScoped<IRoleService, RoleService>();
	}
}
