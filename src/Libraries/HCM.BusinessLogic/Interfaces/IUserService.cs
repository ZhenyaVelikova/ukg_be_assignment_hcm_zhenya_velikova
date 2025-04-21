using HCM.BusinessLogic.Models.People;
using HCM.BusinessLogic.Models.Users;

namespace HCM.BusinessLogic.Interfaces;

public interface IUserService
{
	Task<IEnumerable<UserListResponseModel>> GetPaged(UserFilterRequestModel model);
	Task<IEnumerable<PeopleListResponseModel>> GetPeople(int page = 1, int pageSize = 50, CancellationToken cancellationToken = default);
	Task<UserResponseModel> Get(Guid id);
	Task CreateOrUpdate(Guid? id, UserRequestModel model);
	Task Delete(Guid id);
	Task ChangePassword(Guid id, ChangePasswordModel model);
}
