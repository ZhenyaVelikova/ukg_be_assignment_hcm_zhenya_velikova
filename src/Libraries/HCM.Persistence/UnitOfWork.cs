using Microsoft.EntityFrameworkCore;

namespace HCM.Persistence;
public class UnitOfWork : IUnitOfWork, IDisposable
{
	public DbContext Context { get; }

	public UnitOfWork(DbContext context)
	{
		Context = context;
	}

	public void Commit()
	{
		Context.SaveChanges();
	}

	public async Task CommitAsync()
	{
		await Context.SaveChangesAsync();
	}

	public void Dispose()
	{
		Context.Dispose();
	}
}
