using FluentAssertions;
using HCM.Api.Controllers;
using HCM.BusinessLogic.Interfaces;
using HCM.BusinessLogic.Models.Departments;
using Moq;

namespace HCM.Tests;

public class DepartmentsControllerTests
{
	private readonly Mock<IDepartmentService> _serviceMock;
	private readonly DepartmentssController _controller;

	public DepartmentsControllerTests()
	{
		_serviceMock = new Mock<IDepartmentService>();
		_controller = new DepartmentssController(_serviceMock.Object);
	}

	[Fact]
	public async Task GetAll_WhenCalled_ReturnsAllDepartments()
	{
		// Arrange
		var expected = new[]
		{
				new DepartmentListResponseModel { Id = Guid.NewGuid(), Name = "HR", IsActive = true },
				new DepartmentListResponseModel { Id = Guid.NewGuid(), Name = "IT", IsActive = false }
			};
		_serviceMock
			.Setup(s => s.GetAll())
			.ReturnsAsync(expected);

		// Act
		var result = await _controller.GetAll();

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public async Task Get_WithFilter_CallsServiceAndReturnsPaged()
	{
		// Arrange
		var filter = new DepartmentFilterRequestModel
		{
			IsActive = true,
			SearchTerm = "Fin",
			Page = 2,
			PageSize = 5
		};
		var expected = new List<DepartmentListResponseModel>();
		_serviceMock
			.Setup(s => s.GetPaged(filter))
			.ReturnsAsync(expected);

		// Act
		var result = await _controller.Get(filter);

		// Assert
		_serviceMock.Verify(s => s.GetPaged(filter), Times.Once);
		result.Should().BeSameAs(expected);
	}

	[Fact]
	public async Task Get_ById_ReturnsDepartment()
	{
		// Arrange
		var id = Guid.NewGuid();
		var expected = new DepartmentResponseModel { Id = id, Name = "Finance", IsActive = true };
		_serviceMock
			.Setup(s => s.Get(id))
			.ReturnsAsync(expected);

		// Act
		var result = await _controller.Get(id);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public async Task Create_CallsServiceWithNullId()
	{
		// Arrange
		var request = new DepartmentRequestModel { Name = "Legal", IsActive = true };

		// Act
		await _controller.Create(request);

		// Assert
		_serviceMock.Verify(s => s.CreateOrUpdate(null, request), Times.Once);
	}

	[Fact]
	public async Task Update_CallsServiceWithGivenId()
	{
		// Arrange
		var id = Guid.NewGuid();
		var request = new DepartmentRequestModel { Name = "R&D", IsActive = false };

		// Act
		await _controller.Update(id, request);

		// Assert
		_serviceMock.Verify(s => s.CreateOrUpdate(id, request), Times.Once);
	}

	[Fact]
	public async Task Delete_CallsServiceDelete()
	{
		// Arrange
		var id = Guid.NewGuid();

		// Act
		await _controller.Delete(id);

		// Assert
		_serviceMock.Verify(s => s.Delete(id), Times.Once);
	}
}
