using AutoMapper;
using FluentAssertions;
using Moq;
using ScheduleBarbecue.Tests.Helpers.Features;
using ScheduleBarbecue.Application.Base.Persistence;
using ScheduleBarbecue.Application.Features.Barbecues.Repositories;
using ScheduleBarbecue.Application.Features.Contributions.DTOs;
using ScheduleBarbecue.Application.Features.Contributions.Repositories;
using ScheduleBarbecue.Application.Features.Contributions.Responses;
using ScheduleBarbecue.Application.Features.Contributions.Services;
using ScheduleBarbecue.Application.Features.Contributions.Services.Contracts;
using ScheduleBarbecue.Application.Features.Participants.Repositories;

namespace ScheduleBarbecue.Tests.Application.Features.Contributions;

public class ContributionServiceTests
{
    [Fact(DisplayName = "Should return invalid response when create a contribution and barbecue is not found")]
    public async Task CreateContribution_CreateAInValidContribution_WhenBarbecueNotFound_ShouldReturnInValidResponse()
    {
        // arrange
        var request = ContributionHelper.NewCreateContributionRequest();

        var (_, _, _, service, _) = ContributionContextMock();

        // act
        var (response, createdContribution) = await service.CreateContribution(request);

        // assert
        createdContribution.Should().BeNull();
        response?.IsValid().Should().BeFalse();
        response?.Notifications.Should().Contain(n => n.Description == "Churrasco não localizado.");
    }

    [Fact(DisplayName = "Should return invalid response when create a contribution and participant is not found")]
    public async Task CreateContribution_CreateAInValidContribution_WhenPariticipantNotFound_ShouldReturnInValidResponse()
    {
        // arrange
        var request = ContributionHelper.NewCreateContributionRequest();
        var barbecue = BarbecueHelper.NewBarbecue();

        var (_, barbecueRepositoryMock, _, service, _) = ContributionContextMock();

        barbecueRepositoryMock.Setup(b => b.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(barbecue));

        // act
        var (response, createdContribution) = await service.CreateContribution(request);

        // assert
        createdContribution.Should().BeNull();
        response?.IsValid().Should().BeFalse();
        response?.Notifications.Should().Contain(n => n.Description == "Participante não localizado.");
    }

    [Fact(DisplayName = "Should return invalid response when create a contribution and with drink is true but value with drink inputed is invalid")]
    public async Task CreateContribution_CreateAInValidContribution_WhenValueWithDrinkInvalid_ShouldReturnInValidResponse()
    {
        // arrange
        var request = ContributionHelper.NewCreateContributionRequest();
        request.Value = 10m;

        var barbecue = BarbecueHelper.NewBarbecue();
        var participant = ParticipantHelper.NewParticipant();

        var (_, barbecueRepositoryMock, participantRepositoryMock, service, _) = ContributionContextMock();

        barbecueRepositoryMock.Setup(b => b.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(barbecue));
        participantRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(participant));

        // act
        var (response, createdContribution) = await service.CreateContribution(request);

        // assert
        createdContribution.Should().NotBeNull();
        response?.IsValid().Should().BeFalse();
        response?.Notifications.Should().Contain(n => n.Description == $"Valor da contribuição com bebida informado deve ser igual ou maior que {string.Format("{0:C}", barbecue.SuggestedValueWithDrink)}.");
    }

    [Fact(DisplayName = "Should return invalid response when create a contribution and with drink is false but value without drink inputed is invalid")]
    public async Task CreateContribution_CreateAInValidContribution_WhenValueWithoutDrinkInvalid_ShouldReturnInValidResponse()
    {
        // arrange
        var request = ContributionHelper.NewCreateContributionRequest();
        request.WithDrink = false;
        request.Value = 5m;

        var barbecue = BarbecueHelper.NewBarbecue();
        var participant = ParticipantHelper.NewParticipant();

        var (_, barbecueRepositoryMock, participantRepositoryMock, service, _) = ContributionContextMock();

        barbecueRepositoryMock.Setup(b => b.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(barbecue));
        participantRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(participant));

        // act
        var (response, createdContribution) = await service.CreateContribution(request);

        // assert
        createdContribution.Should().NotBeNull();
        response?.IsValid().Should().BeFalse();
        response?.Notifications.Should().Contain(n => n.Description == $"Valor mínimo da contribuição informado deve ser igual ou maior que {string.Format("{0:C}", barbecue.SuggestedValueWithoutDrink)}.");
    }

    [Fact(DisplayName = "Should return valid response when create a contribution")]
    public async Task CreateContribution_CreateAValidContribution_ShouldReturnValidResponse()
    {
        // arrange
        var request = ContributionHelper.NewCreateContributionRequest();

        var barbecue = BarbecueHelper.NewBarbecue();
        var participant = ParticipantHelper.NewParticipant();

        var (_, barbecueRepositoryMock, participantRepositoryMock, service, _) = ContributionContextMock();

        barbecueRepositoryMock.Setup(b => b.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(barbecue));
        participantRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(participant));

        // act
        var (response, createdContribution) = await service.CreateContribution(request);

        // assert
        createdContribution.Should().NotBeNull();
        createdContribution?.Id.Should().NotBe(Guid.Empty);
        createdContribution?.BarbecueId.Should().NotBe(Guid.Empty);
        createdContribution?.ParticipantId.Should().NotBe(Guid.Empty);
        createdContribution?.Value.Should().Be(request.Value);
        response?.IsValid().Should().BeTrue();
        response?.Notifications.Should().HaveCount(0);
    }

    [Fact(DisplayName = "Should return valid response when remove a contribution")]
    public async Task RemoveContribution_RemoveAValidContribution_ShouldReturnValidResponse()
    {
        // arrange
        var request = ContributionHelper.NewContribution();

        var (contributionRepositoryMock, _, _, service, _) = ContributionContextMock();

        contributionRepositoryMock.Setup(c => c.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(request));

        // act
        var response = await service.RemoveContribution(request.Id);

        // assert
        response?.IsValid().Should().BeTrue();
        response?.Notifications.Should().HaveCount(0);
    }

    [Fact(DisplayName = "Should return status 'Ok' when get all participant for contribution valid")]
    public async Task GetAllParticipantByBarbecueId_GetAllStatusValid_ShouldReturnStatusIsValid()
    {
        // arrange
        var (contributionRepositoryMock, _, _, service, mapper) = ContributionContextMock();

        var listContributionParticipantDto = ContributionHelper.ListContributionParticipantDto();
        var listContributionParticipantResponse = ContributionHelper.ListContributionParticipantResponse();

        contributionRepositoryMock.Setup(r => r.GetAllParticipantByBarbecueIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(listContributionParticipantDto));
        mapper.Setup(m => m.Map<List<ContributionParticipantResponse?>>(It.IsAny<List<ContributionParticipantDto>>())).Returns(listContributionParticipantResponse);

        // act
        var (response, listedBarbecue) = await service.GetAllParticipantByBarbecueId(Guid.Empty);

        // assert
        listedBarbecue.Should().NotBeNull();
        listedBarbecue?.Count.Should().Be(2);
        listedBarbecue?.Should().BeEquivalentTo(listContributionParticipantResponse);
        response?.IsValid().Should().BeTrue();
        response?.Notifications.Should().HaveCount(0);
    }

    private static (
        Mock<IContributionRepository> contributionRepositoryMock,
        Mock<IBarbecueRepository> barbecueRepositoryMock,
        Mock<IParticipantRepository> participantRepositoryMock,
        IContributionService serviceMock,
        Mock<IMapper> mapperMock) ContributionContextMock()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperMock = new Mock<IMapper>();
        var contributionRepository = new Mock<IContributionRepository>();
        var barbecueRepositoryMock = new Mock<IBarbecueRepository>();
        var participantRepositoryMock = new Mock<IParticipantRepository>();

        unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

        var contributionService = new ContributionService(unitOfWorkMock.Object, mapperMock.Object, contributionRepository.Object, barbecueRepositoryMock.Object, participantRepositoryMock.Object);

        return (contributionRepository, barbecueRepositoryMock, participantRepositoryMock, contributionService, mapperMock);
    }
}