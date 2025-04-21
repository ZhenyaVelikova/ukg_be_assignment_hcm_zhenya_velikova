using HCM.BusinessLogic.Models.Departments;

namespace HCM.BusinessLogic.Interfaces;

public interface IDepartmentService
{
	Task<IEnumerable<DepartmentListResponseModel>> GetAll();
	Task<IEnumerable<DepartmentListResponseModel>> GetPaged(DepartmentFilterRequestModel model);
	Task<DepartmentResponseModel> Get(Guid id);
	Task CreateOrUpdate(Guid? id, DepartmentRequestModel model);
	Task Delete(Guid id);
}
