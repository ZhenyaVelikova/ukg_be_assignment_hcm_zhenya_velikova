using HCM.BusinessLogic.Interfaces;
using HCM.BusinessLogic.Models.Departments;
using HCM.BusinessLogic.Models.People;
using HCM.Core.Constants;
using HCM.Persistence.Entities;
using HCM.Persistence.Interfaces;
using HCM.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HCM.BusinessLogic.Services
{
	public class DepartmentService : IDepartmentService
	{
		private readonly IDepartmentRepository departmentRepository;
		private readonly IUnitOfWork unitOfWork;

		public DepartmentService(
			IDepartmentRepository departmentRepository,
			IUnitOfWork unitOfWork)
		{
			this.departmentRepository = departmentRepository;
			this.unitOfWork = unitOfWork;
		}

		public async Task<IEnumerable<DepartmentListResponseModel>> GetAll()
		{
			var departments = await departmentRepository.GetQueryable()
				.Where(x => x.IsActive)
				.OrderBy(x => x.Name)
				.ToListAsync();
			return departments.Select(x => new DepartmentListResponseModel
			{
				Id = x.Id,
				Name = x.Name,
				IsActive = x.IsActive
			});
		}
		public async Task<IEnumerable<DepartmentListResponseModel>> GetPaged(DepartmentFilterRequestModel model)
		{
			var query = departmentRepository.GetQueryable();

			if (model.IsActive.HasValue)
			{
				query = query.Where(x => x.IsActive == model.IsActive.Value);
			}

			if (!string.IsNullOrWhiteSpace(model.SearchTerm))
			{
				query = query.Where(x => x.Name.Contains(model.SearchTerm));
			}

			query = query
				.OrderBy(x => x.Name)
				.Skip((model.Page - 1) * model.PageSize)
				.Take(model.PageSize);

			var departments = await query.ToListAsync();

			return departments.Select(x => new DepartmentListResponseModel
			{
				Id = x.Id,
				Name = x.Name,
				IsActive = x.IsActive
			});
		}

		public async Task<DepartmentResponseModel> Get(Guid id)
		{
			var department = await departmentRepository
				.GetQueryable()
				.Include(d => d.People)
				.FirstOrDefaultAsync(d => d.Id == id)
				?? throw new Exception(MessageConstants.DepartmentNotFound);

			return new DepartmentResponseModel
			{
				Id = department.Id,
				Name = department.Name,
				IsActive = department.IsActive,
				People = department.People.Select(person => new PersonResponseModel
				{
					Id = person.Id,
					FirstName = person.FirstName!,
					LastName = person.LastName!,
					Email = person.Email!,
					StartDate = person.StartDate,
					TerminationDate = person.TerminationDate,
					PersonType = person.PersonType,
					ReportsToId = person.ReportsToId,
					DepartmentId = person.DepartmentId,
					PositionId = person.PositionId
				}).ToList()
			};
		}

		public async Task CreateOrUpdate(Guid? id, DepartmentRequestModel model)
		{
			if (string.IsNullOrWhiteSpace(model.Name))
				throw new Exception(MessageConstants.DepartmentNameMustNotBeEmpty);

			if (model.Name.Length > ValueLengthConstants.Department.NameMaxLength)
				throw new Exception($"Name must be less than {ValueLengthConstants.Department.NameMaxLength} characters.");

			if (id.HasValue)
			{
				var existing = await departmentRepository.GetByIdAsync(id.Value)
					?? throw new Exception(MessageConstants.DepartmentNotFound);

				existing.Name = model.Name;
				existing.IsActive = model.IsActive;

				await departmentRepository.UpdateAsync(existing);
			}
			else
			{
				var newDepartment = new DepartmentEntity
				{
					Name = model.Name,
					IsActive = model.IsActive
				};

				await departmentRepository.AddAsync(newDepartment);
			}

			await unitOfWork.CommitAsync();
		}

		public async Task Delete(Guid id)
		{
			var department = await departmentRepository.GetByIdAsync(id)
				?? throw new Exception(MessageConstants.DepartmentNotFound);

			if (!department.IsActive)
				throw new Exception(MessageConstants.DepartmentAlreadyInactive);

			department.IsActive = false;
			department.Name += $" -{MessageConstants.Inactive}";

			await departmentRepository.UpdateAsync(department);
			await unitOfWork.CommitAsync();
		}
	}

}
