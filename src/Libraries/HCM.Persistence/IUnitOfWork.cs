using Microsoft.EntityFrameworkCore;

namespace HCM.Persistence;

public interface IUnitOfWork : IDisposable
{
	DbContext Context { get; }

	void Commit();

	Task CommitAsync();
}
