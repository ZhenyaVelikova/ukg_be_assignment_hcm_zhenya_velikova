using HCM.Persistence.Entities;

namespace HCM.Persistence.Interfaces;

public interface IUserRepository : IGenericRepository<UserEntity>
{
	Task<UserEntity?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
	Task<bool> CheckUsernameExists(string username, Guid? id);
}
