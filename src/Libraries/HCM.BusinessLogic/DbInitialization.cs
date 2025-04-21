using HCM.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HCM.BusinessLogic;

public static class DbInitialization
{
	public static void DbInitialize(this IServiceProvider serviceProvider)
	{
		using var scope = serviceProvider.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<HCMDbContext>();
		context.Database.Migrate();
	}
}

