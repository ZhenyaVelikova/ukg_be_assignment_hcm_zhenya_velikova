using HCM.BusinessLogic.Interfaces;
using HCM.BusinessLogic.Models.People;
using HCM.BusinessLogic.Models.Positions;
using HCM.Core.Constants;
using HCM.Persistence;
using HCM.Persistence.Entities;
using HCM.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HCM.BusinessLogic.Services;

public class PositionService : IPositionService
{
	private readonly IPositionRepository positionRepository;
	private readonly IUnitOfWork unitOfWork;

	public PositionService(
		IPositionRepository positionRepository,
		IUnitOfWork unitOfWork)
	{
		this.positionRepository = positionRepository;
		this.unitOfWork = unitOfWork;
	}

	public async Task<IEnumerable<PositionListResponseModel>> GetAll()
	{
		var positions = await positionRepository.GetQueryable()
		.Where(x => x.IsActive)
		.OrderBy(x => x.Name)
		.ToListAsync();
		return positions.Select(x => new PositionListResponseModel
		{
			Id = x.Id,
			Name = x.Name,
			IsActive = x.IsActive
		});
	}
	public async Task<IEnumerable<PositionListResponseModel>> GetPaged(PositionFilterRequestModel model)
	{
		var query = positionRepository.GetQueryable();

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

		var positions = await query.ToListAsync();

		return positions.Select(x => new PositionListResponseModel
		{
			Id = x.Id,
			Name = x.Name,
			IsActive = x.IsActive
		});
	}


	public async Task<PositionResponseModel> Get(Guid id)
	{
		var position = await positionRepository
			.GetQueryable()
			.Include(p => p.People)
			.FirstOrDefaultAsync(p => p.Id == id)
			?? throw new Exception(MessageConstants.PositionNotFound);

		return new PositionResponseModel
		{
			Id = position.Id,
			Name = position.Name,
			IsActive = position.IsActive,
			People = position.People.Select(person => new PersonResponseModel
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

	public async Task CreateOrUpdate(Guid? id, PositionRequestModel model)
	{
		if (string.IsNullOrWhiteSpace(model.Name))
		{
			throw new Exception(MessageConstants.PositionNameMustNotBeEmpty);
		}

		if (model.Name.Length > ValueLengthConstants.Position.NameMaxLength)
		{
			throw new Exception($"Name must be less than {ValueLengthConstants.Position.NameMaxLength} characters.");
		}

		if (id.HasValue)
		{
			var existing = await positionRepository.GetByIdAsync(id.Value) ?? throw new Exception(MessageConstants.PositionNotFound);
			existing.Name = model.Name;
			existing.IsActive = model.IsActive;

			await positionRepository.UpdateAsync(existing);
		}
		else
		{
			var newPosition = new PositionEntity
			{
				Name = model.Name,
				IsActive = model.IsActive
			};

			await positionRepository.AddAsync(newPosition);
		}

		await unitOfWork.CommitAsync();
	}

	public async Task Delete(Guid id)
	{
		var position = await positionRepository.GetByIdAsync(id) ?? throw new Exception(MessageConstants.PositionNotFound);

		if (!position.IsActive)
			throw new Exception(MessageConstants.PositionAlreadyInactive);

		position.IsActive = false;
		position.Name += $" -{MessageConstants.Inactive}";

		await positionRepository.UpdateAsync(position);
		await unitOfWork.CommitAsync();
	}
}
