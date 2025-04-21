using HCM.Persistence.Entities;
using HCM.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HCM.Persistence.Repositories;
public class UserRepository : GenericRepository<UserEntity>, IUserRepository
{
	public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
	{
	}
	public async Task<UserEntity?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
	{
		return await dbSet.FirstOrDefaultAsync(u => u.UserName == userName && u.IsActive, cancellationToken)
			.ConfigureAwait(false);
	}

	public async Task<bool> CheckUsernameExists(string username, Guid? id)
	{
		var count = await dbSet.AsNoTracking().Where(x => x.IsActive && x.UserName == username).CountAsync();
		return count > 0;
	}

	

}
