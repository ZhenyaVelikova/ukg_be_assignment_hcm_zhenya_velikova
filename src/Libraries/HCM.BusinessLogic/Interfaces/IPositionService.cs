using HCM.BusinessLogic.Models.Positions;

namespace HCM.BusinessLogic.Interfaces;

public interface IPositionService
{
	Task<IEnumerable<PositionListResponseModel>> GetAll();
	Task<IEnumerable<PositionListResponseModel>> GetPaged(PositionFilterRequestModel model);
	Task<PositionResponseModel> Get(Guid id);
	Task CreateOrUpdate(Guid? id, PositionRequestModel model);
	Task Delete(Guid id);
}
