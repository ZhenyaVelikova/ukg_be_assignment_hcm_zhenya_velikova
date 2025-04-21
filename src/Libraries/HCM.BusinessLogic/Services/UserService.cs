using HCM.BusinessLogic.Interfaces;
using HCM.BusinessLogic.Models.People;
using HCM.BusinessLogic.Models.Users;
using HCM.Core.Constants;
using HCM.Core.Enums;
using HCM.Core.Helpers;
using HCM.Persistence;
using HCM.Persistence.Entities;
using HCM.Persistence.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace HCM.BusinessLogic.Services;

public class UserService : IUserService
{
	private readonly IUserRepository userRepository;
	private readonly IPersonRepository personRepository;
	private readonly IRoleRepository roleRepository;
	private readonly IPositionRepository positionRepository;
	private readonly IDepartmentRepository departmentRepository;
	private readonly IUnitOfWork unitOfWork;
	private readonly IHttpContextAccessor httpContextAccessor;

	public UserService(
		   IUserRepository userRepository,
		   IPersonRepository personRepository,
		   IRoleRepository roleRepository,
		   IPositionRepository positionRepository,
		   IDepartmentRepository departmentRepository,
		   IUnitOfWork unitOfWork,
		   IHttpContextAccessor httpContextAccessor
		  )
	{
		this.userRepository = userRepository;
		this.personRepository = personRepository;
		this.roleRepository = roleRepository;
		this.positionRepository = positionRepository;
		this.departmentRepository = departmentRepository;
		this.unitOfWork = unitOfWork;
		this.httpContextAccessor = httpContextAccessor;
	}

	public async Task<IEnumerable<UserListResponseModel>> GetPaged(UserFilterRequestModel model)
	{
		var query = await userRepository.GetAllAsync(
	  predicate: x =>
		  (!model.IsActive.HasValue || x.IsActive == model.IsActive.Value) &&
		  (string.IsNullOrWhiteSpace(model.SearchTerm) || x.UserName.Contains(model.SearchTerm)),
	  include: query => query
		  .Include(x => x.UserRoles)
			  .ThenInclude(ur => ur.Role),
	  cancellationToken: CancellationToken.None
		 );

		if (model.IsActive.HasValue)
		{
			query = query.Where(x => x.IsActive == model.IsActive.Value);
		}

		if (!string.IsNullOrWhiteSpace(model.SearchTerm))
		{
			query = query.Where(x => x.UserName!.Contains(model.SearchTerm));
		}

		query = query
			.OrderBy(x => x.UserName)
			.Skip((model.Page - 1) * model.PageSize)
			.Take(model.PageSize);

		var users = query.ToList();

		return users.Select(x =>
		{
			var role = x.UserRoles?.FirstOrDefault()?.Role;
			return new UserListResponseModel
			{
				Id = x.Id,
				UserName = x.UserName!,
				IsActive = x.IsActive,
				RoleId = role?.Id,
				RoleName = role?.Name
			};
		});
	}

	public async Task<IEnumerable<PeopleListResponseModel>> GetPeople(int page = 1, int pageSize = 50, CancellationToken cancellationToken = default)
	{
		var people = await personRepository.GetAllAsync(
		predicate: p => p.IsActive,
		include: q => q
		   .Include(p => p.Position)
		   .Include(p => p.Department),
		cancellationToken: cancellationToken
		   );

		var paged = people
		   .OrderBy(p => p.LastName).ThenBy(p => p.FirstName)
		   .Skip((page - 1) * pageSize)
		   .Take(pageSize);

		return paged.Select(p => new PeopleListResponseModel
		{
			PersonId = p.UserId,
			FullName = $"{p.FirstName} {p.LastName}",
			Email = p.Email,
			StartDate = p.StartDate,
			TerminationDate = p.TerminationDate?.ToString("yyyy-MM-dd"),
			PositionName = p.Position?.Name ?? "—",
			DepartmentName = p.Department?.Name ?? "—"
		});
	}

	public async Task<UserResponseModel> Get(Guid id)
	{
		var user = await userRepository
			.GetQueryable()
			.Include(x => x.UserRoles)
				.ThenInclude(ur => ur.Role)
			.FirstOrDefaultAsync(x => x.Id == id)
			?? throw new Exception(MessageConstants.UserNotFound);

		var person = (await personRepository.GetAllAsync(p => p.UserId == user.Id)).FirstOrDefault();

		var role = user.UserRoles.FirstOrDefault()?.Role;

		return new UserResponseModel
		{
			Id = user.Id,
			UserName = user.UserName!,
			IsActive = user.IsActive,
			RoleId = role.Id,
			RoleName = role?.Name,
			Person = person == null ? null : new PersonResponseModel
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
			}
		};
	}

	public async Task CreateOrUpdate(Guid? id, UserRequestModel model)
	{
		UserEntity user;
		var isCreate = !id.HasValue;
		var currentUserId = GetCurrentUserId();
		var isSelf = id.HasValue && id.Value == currentUserId;

		if (isCreate)
		{
			if (string.IsNullOrWhiteSpace(model.UserName) && model.Person != null)
				model.UserName = model.Person.Email;

			await ValidateUserAndPerson(model, true);

			if (await userRepository.GetByUserNameAsync(model.UserName) != null)
				throw new Exception(MessageConstants.UsernameAlreadyTaken);

			user = new UserEntity
			{
				UserName = model.UserName,
				Password = PasswordHasherHelper.Hash(model.Password),
				IsActive = model.IsActive
			};
			await userRepository.AddAsync(user);

			user.UserRoles.Add(new UserRoleEntity
			{
				UserId = user.Id,
				RoleId = model.RoleId
			});

			await unitOfWork.CommitAsync();
		}
		else
		{
			user = await userRepository.GetByIdAsync(
				id.Value,
				q => q.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
			) ?? throw new Exception(MessageConstants.UserNotFound);

			await ValidateUserAndPerson(model, isSelf);

			if (model.UserName != user.UserName &&
				await userRepository.CheckUsernameExists(model.UserName, user.Id))
				throw new Exception(MessageConstants.UsernameAlreadyTaken);
			if(!string.IsNullOrEmpty(model.Password))
			{
				user.UserName = model.UserName;
				user.Password = PasswordHasherHelper.Hash(model.Password);
			}
			
			user.IsActive = model.IsActive;

			var existingRole = user.UserRoles.FirstOrDefault(ur => ur.RoleId == model.RoleId);
			if (existingRole != null)
				existingRole.RoleId = model.RoleId;
			else
				user.UserRoles.Add(new UserRoleEntity
				{
					UserId = user.Id,
					RoleId = model.RoleId
				});

			await userRepository.UpdateAsync(user);

			await unitOfWork.CommitAsync();
		}

		if (model.Person != null)
		{
			Guid? managerPersonId = null;
			if (model.Person.ReportsToId.HasValue)
			{
				var managerPerson = (await personRepository
							   .GetAllAsync(p => p.UserId == model.Person.ReportsToId.Value))
							   .FirstOrDefault();
				if (managerPerson == null)
					throw new Exception("The selected manager does not have a Person record.");
				managerPersonId = managerPerson.Id;
			}
			var existingPerson = (await personRepository
				.GetAllAsync(p => p.UserId == user.Id))
				.FirstOrDefault();

			if (existingPerson != null)
			{
				existingPerson.FirstName = model.Person.FirstName;
				existingPerson.LastName = model.Person.LastName;
				existingPerson.Email = model.Person.Email;
				existingPerson.PositionId = model.Person.PositionId;
				existingPerson.DepartmentId = model.Person.DepartmentId;
				existingPerson.StartDate = model.Person.StartDate;
				existingPerson.TerminationDate = model.Person.TerminationDate;
				existingPerson.PersonType = model.Person.PersonType;
				existingPerson.ReportsToId = managerPersonId;

				await personRepository.UpdateAsync(existingPerson);
			}
			else
			{
				await personRepository.AddAsync(new PersonEntity
				{
					UserId = user.Id,
					FirstName = model.Person.FirstName,
					LastName = model.Person.LastName,
					Email = model.Person.Email,
					PositionId = model.Person.PositionId,
					DepartmentId = model.Person.DepartmentId,
					StartDate = model.Person.StartDate,
					TerminationDate = model.Person.TerminationDate,
					PersonType = model.Person.PersonType,
					ReportsToId = managerPersonId,
					IsActive = true
				});
			}

			await unitOfWork.CommitAsync();
		}
	}

	public async Task Delete(Guid id)
	{
		var user = await userRepository.GetByIdAsync(id) ?? throw new Exception(MessageConstants.UserNotFound);

		if (!user.IsActive)
			throw new Exception(MessageConstants.UserAlreadyInactive);

		user.IsActive = false;
		user.UserName += $" -{MessageConstants.Inactive}";

		var person = await personRepository.GetAllAsync(p => p.UserId == user.Id);
		if (person.Any())
		{
			var personEntity = person.FirstOrDefault();
			personEntity.IsActive = false;

			await personRepository.UpdateAsync(personEntity);
		}

		await userRepository.UpdateAsync(user);
		await unitOfWork.CommitAsync();
	}

	public async Task ChangePassword(Guid id, ChangePasswordModel model)
	{
		var currentUserId = GetCurrentUserId();
		if (currentUserId != id)
			throw new Exception("You can only change your own password.");

		var user = await userRepository.GetByIdAsync(id)
				   ?? throw new Exception(MessageConstants.UserNotFound);

		if (!PasswordHasherHelper.Verify(model.CurrentPassword, user.Password))
			throw new Exception("Current password is incorrect.");

		user.Password = PasswordHasherHelper.Hash(model.NewPassword);
		await userRepository.UpdateAsync(user);
		await unitOfWork.CommitAsync();
	}

	private async Task ValidateUserAndPerson(UserRequestModel model, bool requirePassword)
	{
		if (string.IsNullOrWhiteSpace(model.UserName) || model.UserName.Length > ValueLengthConstants.User.UsernameMaxLength)
		{
			throw new Exception($"UserName must not be empty and should be less than {ValueLengthConstants.User.UsernameMaxLength} characters.");
		}

		if (requirePassword && string.IsNullOrWhiteSpace(model.Password))
			throw new Exception("Password cannot be empty.");


		var role = await roleRepository.GetByIdAsync(model.RoleId);
		if (role == null)
		{
			throw new Exception("Invalid role specified.");
		}

		if (model.Person != null)
		{
			if (string.IsNullOrWhiteSpace(model.Person.FirstName) || model.Person.FirstName.Length > ValueLengthConstants.Person.FirstNameMaxLength)
			{
				throw new Exception($"FirstName must not be empty and should be less than {ValueLengthConstants.Person.FirstNameMaxLength} characters.");
			}

			if (string.IsNullOrWhiteSpace(model.Person.LastName) || model.Person.LastName.Length > ValueLengthConstants.Person.LastNameMaxLength)
			{
				throw new Exception($"LastName must not be empty and should be less than {ValueLengthConstants.Person.LastNameMaxLength} characters.");
			}

			if (string.IsNullOrWhiteSpace(model.Person.Email) || model.Person.Email.Length > ValueLengthConstants.Person.EmailMaxLength)
			{
				throw new Exception($"Email must not be empty and should be less than {ValueLengthConstants.Person.EmailMaxLength} characters.");
			}

			try
			{
				var addr = new System.Net.Mail.MailAddress(model.Person.Email);
				if (addr.Address != model.Person.Email)
					throw new Exception("Invalid email format.");
			}
			catch
			{
				throw new Exception("Invalid email format.");
			}

			if (model.Person.StartDate == DateTime.MinValue)
			{
				throw new Exception("StartDate cannot be empty.");
			}

			if (!Enum.IsDefined(typeof(PersonType), model.Person.PersonType))
			{
				throw new Exception("Invalid Person Type.");
			}

			if (model.Person.DepartmentId != Guid.Empty)
			{
				var departmentExists = await departmentRepository.ExistsAsync(model.Person.DepartmentId);
				if (!departmentExists)
				{
					throw new Exception("Invalid Department specified.");
				}
			}

			if (model.Person.PositionId != Guid.Empty)
			{
				var positionExists = await positionRepository.ExistsAsync(model.Person.PositionId);
				if (!positionExists)
				{
					throw new Exception("Invalid Position specified.");
				}
			}
		}
	}

	private Guid? GetCurrentUserId()
	{
		var u = httpContextAccessor.HttpContext?.User;
		var claim = u?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		return Guid.TryParse(claim, out var id) ? id : (Guid?)null;
	}

}


