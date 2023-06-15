using AutoMapper;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using ScheduleBarbecue.Tests.Helpers.Features;
using ScheduleBarbecue.Application.Base.Persistence;
using ScheduleBarbecue.Application.Features.Barbecues;
using ScheduleBarbecue.Application.Features.Barbecues.DTOs;
using ScheduleBarbecue.Application.Features.Barbecues.Repositories;
using ScheduleBarbecue.Application.Features.Barbecues.Responses;
using ScheduleBarbecue.Application.Features.Barbecues.Services;
using ScheduleBarbecue.Application.Features.Barbecues.Services.Contracts;

namespace ScheduleBarbecue.Tests.Application.Features.Barbecues;

public class BarbecueServiceTests
{
    [Fact(DisplayName = "Should return valid response when create a barbecue")]
    public async Task CreateBarbecue_CreateAValidBarbecue_ShouldReturnValidResponse()
    {
        // arrange
        var request = BarbecueHelper.NewCreateBarbecueRequest();

        var (_, service, _) = BarbecueContextMock();

        // act
        var (response, createdBarbecue) = await service.CreateBarbecue(request);

        // assert
        createdBarbecue.Should().NotBeNull();
        createdBarbecue?.Id.Should().NotBe(Guid.Empty);
        createdBarbecue?.Name.Should().Be(request.Name);
        createdBarbecue?.Description.Should().Be(request.Description);
        createdBarbecue?.AdditionalNote.Should().Be(request.AdditionalNote);
        createdBarbecue?.SuggestedValueWithDrink.Should().Be(request.SuggestedValueWithDrink);
        createdBarbecue?.SuggestedValueWithoutDrink.Should().Be(request.SuggestedValueWithoutDrink);
        createdBarbecue?.HasSuggestedValue.Should().Be(request.HasSuggestedValue);
        response?.IsValid().Should().BeTrue();
        response?.Notifications.Should().HaveCount(0);
    }

    [Fact(DisplayName = "Should return status 'Ok' when get all barbecue valid")]
    public async Task GetAllBarbecue_GetAllStatusValid_ShouldReturnStatusIsValid()
    {
        // arrange
        var (repository, service, mapper) = BarbecueContextMock();

        var listBarbecue = BarbecueHelper.ListBarbecue();
        var listBarbecueResponse = BarbecueHelper.ListBarbecueResponse();

        repository.Setup(r => r.GetAllAsync()).Returns(Task.FromResult(listBarbecue));
        mapper.Setup(m => m.Map<List<BarbecueResponse?>>(It.IsAny<List<Barbecue>>())).Returns(listBarbecueResponse);

        // act
        var (response, listedBarbecue) = await service.GetAllBarbecue();

        // assert
        listedBarbecue.Should().NotBeNull();
        listedBarbecue?.Count.Should().Be(2);
        listedBarbecue?.Should().BeEquivalentTo(listBarbecueResponse);
        response?.IsValid().Should().BeTrue();
        response?.Notifications.Should().HaveCount(0);
    }

    [Fact(DisplayName = "Should return status 'Ok' when get all compendious barbecue valid")]
    public async Task GetAllOrOneBarbecueCompendious_GetAllBarbecueCompendiousStatusValid_ShouldReturnStatusIsValid()
    {
        // arrange
        var (repository, service, mapper) = BarbecueContextMock();

        var listBarbecueDtoQuery = BarbecueHelper.ListBarbecueDto().AsQueryable().BuildMock();
        var listBarbecueCompendiousResponse = BarbecueHelper.ListBarbecueCompendiousResponse();

        repository.Setup(r => r.GetAllOrOneCompendiousAsync(It.IsAny<Guid?>())).Returns(listBarbecueDtoQuery);
        mapper.Setup(m => m.Map<List<BarbecueCompendiousResponse?>>(It.IsAny<List<BarbecueDto>>())).Returns(listBarbecueCompendiousResponse);

        // act
        var (response, listedBarbecue) = await service.GetAllOrOneBarbecueCompendious(null);

        // assert
        listedBarbecue.Should().NotBeNull();
        listedBarbecue?.Count.Should().Be(2);
        listedBarbecue?.Should().BeEquivalentTo(listBarbecueCompendiousResponse);
        response?.IsValid().Should().BeTrue();
        response?.Notifications.Should().HaveCount(0);
    }

    private static (Mock<IBarbecueRepository> repositoryMock, IBarbecueService serviceMock, Mock<IMapper> mapperMock) BarbecueContextMock()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperMock = new Mock<IMapper>();
        var barbecueRepositoryMock = new Mock<IBarbecueRepository>();

        unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

        var barbecueService = new BarbecueService(unitOfWorkMock.Object, mapperMock.Object, barbecueRepositoryMock.Object);

        return (barbecueRepositoryMock, barbecueService, mapperMock);
    }
}