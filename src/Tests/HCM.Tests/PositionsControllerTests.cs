using FluentAssertions;
using HCM.Api.Controllers;
using HCM.BusinessLogic.Interfaces;
using HCM.BusinessLogic.Models.Positions;
using Moq;

namespace HCM.Tests;

public class PositionsControllerTests
{
	private readonly Mock<IPositionService> _svc = new();
	private readonly PositionsController _ctrl;

	public PositionsControllerTests()
	{
		_ctrl = new PositionsController(_svc.Object);
	}

	[Fact]
	public async Task GetAll_ReturnsServiceResult()
	{
		// Arrange
		var sample = new[]
		{
			new PositionListResponseModel { Id = Guid.NewGuid(), Name = "Dev", IsActive = true }
		};
		_svc.Setup(s => s.GetAll()).ReturnsAsync(sample);

		// Act
		var result = await _ctrl.GetAll();

		// Assert
		result.Should().BeEquivalentTo(sample);
	}

	[Fact]
	public async Task GetPaged_PassesFilterToService()
	{
		var filter = new PositionFilterRequestModel { IsActive = true, Page = 2, PageSize = 10 };
		var expected = Enumerable.Empty<PositionListResponseModel>();
		_svc.Setup(s => s.GetPaged(filter)).ReturnsAsync(expected);

		var result = await _ctrl.Get(filter);

		_svc.Verify(s => s.GetPaged(filter), Times.Once);
		result.Should().BeSameAs(expected);
	}

	[Fact]
	public async Task GetById_ReturnsSinglePosition()
	{
		var id = Guid.NewGuid();
		var model = new PositionResponseModel { Id = id, Name = "CTO" };
		_svc.Setup(s => s.Get(id)).ReturnsAsync(model);

		var result = await _ctrl.Get(id);

		result.Should().Be(model);
	}

	[Fact]
	public async Task Create_CallsCreateOrUpdateWithNullId()
	{
		var req = new PositionRequestModel { Name = "CFO", IsActive = true };
		await _ctrl.Create(req);

		_svc.Verify(s => s.CreateOrUpdate(null, req), Times.Once);
	}

	[Fact]
	public async Task Update_CallsCreateOrUpdateWithId()
	{
		var id = Guid.NewGuid();
		var req = new PositionRequestModel { Name = "Intern", IsActive = false };
		await _ctrl.Update(id, req);

		_svc.Verify(s => s.CreateOrUpdate(id, req), Times.Once);
	}

	[Fact]
	public async Task Delete_CallsServiceDelete()
	{
		var id = Guid.NewGuid();
		await _ctrl.Delete(id);

		_svc.Verify(s => s.Delete(id), Times.Once);
	}
}
