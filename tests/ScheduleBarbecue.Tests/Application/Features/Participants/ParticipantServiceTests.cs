using AutoMapper;
using FluentAssertions;
using Moq;
using ScheduleBarbecue.Tests.Helpers.Features;
using ScheduleBarbecue.Application.Base.Persistence;
using ScheduleBarbecue.Application.Features.Participants;
using ScheduleBarbecue.Application.Features.Participants.Repositories;
using ScheduleBarbecue.Application.Features.Participants.Responses;
using ScheduleBarbecue.Application.Features.Participants.Services;
using ScheduleBarbecue.Application.Features.Participants.Services.Contracts;

namespace ScheduleBarbecue.Tests.Application.Features.Participants;

public class ParticipantServiceTests
{
    [Fact(DisplayName = "Should return valid response when create a participant")]
    public async Task CreateParticipant_CreateAValidParticipant_ShouldReturnValidResponse()
    {
        // arrange
        var request = ParticipantHelper.NewCreateParticipantRequest();

        var (_, service, _) = ParticipantContextMock();

        // act
        var (response, createdParticipant) = await service.CreateParticipant(request);

        // assert
        createdParticipant.Should().NotBeNull();
        createdParticipant?.Id.Should().NotBe(Guid.Empty);
        createdParticipant?.Name.Should().Be(request.Name);
        response?.IsValid().Should().BeTrue();
        response?.Notifications.Should().HaveCount(0);
    }

    [Fact(DisplayName = "Should return status 'Ok' when get all participant valid")]
    public async Task GetAllParticipant_GetAllStatusValid_ShouldReturnStatusIsValid()
    {
        // arrange
        var (repository, service, mapper) = ParticipantContextMock();

        var listParticipant = ParticipantHelper.ListParticipant();
        var listParticipantResponse = ParticipantHelper.ListParticipantResponse();

        repository.Setup(r => r.GetAllAsync()).Returns(Task.FromResult(listParticipant));
        mapper.Setup(m => m.Map<List<ParticipantResponse?>>(It.IsAny<List<Participant>>())).Returns(listParticipantResponse);

        // act
        var (response, listedParticipant) = await service.GetAllParticipant();

        // assert
        listedParticipant.Should().NotBeNull();
        listedParticipant?.Count.Should().Be(2);
        listedParticipant?.Should().BeEquivalentTo(listParticipantResponse);
        response?.IsValid().Should().BeTrue();
        response?.Notifications.Should().HaveCount(0);
    }

    private static (Mock<IParticipantRepository> repositoryMock, IParticipantService serviceMock, Mock<IMapper> mapperMock) ParticipantContextMock()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperMock = new Mock<IMapper>();
        var participantRepositoryMock = new Mock<IParticipantRepository>();

        unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

        var participantService = new ParticipantService(unitOfWorkMock.Object, mapperMock.Object, participantRepositoryMock.Object);

        return (participantRepositoryMock, participantService, mapperMock);
    }
}