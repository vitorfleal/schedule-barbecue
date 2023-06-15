using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ScheduleBarbecue.Tests.Helpers;
using ScheduleBarbecue.Tests.Helpers.Features;
using ScheduleBarbecue.Application.Base.Persistence;
using ScheduleBarbecue.Application.Features.Participants;
using ScheduleBarbecue.Application.Features.Participants.Repositories;
using ScheduleBarbecue.Application.Features.Participants.Responses;
using System.Net;
using System.Net.Http.Json;

namespace ScheduleBarbecue.Tests.Api.Controllers.v1;

public class ParticipantControllerTests : IntegrationTestBaseWithFakeJWT<Program>
{
    private readonly HttpClient _client;

    public ParticipantControllerTests()
    {
        _client = GetTestAppClient();
    }

    [Fact(DisplayName = "Should return status 'Created' when create a Participant valid")]
    public async Task CreateParticipant_CreateAParticipantValid_ShouldReturnStatusCreated()
    {
        //Arrange
        var participant = ParticipantHelper.NewCreateParticipantRequest();

        //Act
        var response = await _client.PostAsJsonAsync("/v1/participant/", participant);

        //Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();

        var participantResponse = JsonConvert.DeserializeObject<ParticipantResponse>(content);
        participantResponse.Should().NotBeNull();

        var participantDb = await GetParticipantById(participantResponse.Id);
        participantDb.Should().NotBeNull();

        participantResponse?.Id.Should().Be(participantDb.Id);
        participantResponse?.Name.Should().BeEquivalentTo(participantDb.Name);
    }

    [Fact(DisplayName = "Should return status 'Ok' when get all Participant valid")]
    public async Task GetAllParticipant_GetAllParticipantValid_ShouldReturnStatusIsValid()
    {
        //Arrange
        var participant = await CreateParticipant();

        //Act
        var response = await _client.GetAsync("/v1/participant");

        //Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();

        var participantResponse = JsonConvert.DeserializeObject<IEnumerable<ParticipantResponse>>(content);
        participantResponse.Should().NotBeNull();
        participantResponse.Should().HaveCount(1);
        participantResponse?.Select(x => x.Id).Should().Equal(participant.Id);
        participantResponse?.Select(x => x.Name).Should().BeEquivalentTo(participant.Name);
    }

    private async Task<Participant?> CreateParticipant()
    {
        using var scope = ServiceProvider?.CreateScope();

        if (scope is null)
            return null;

        var participant = ParticipantHelper.NewParticipant();
        var participantRepository = scope.ServiceProvider.GetRequiredService<IParticipantRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        await participantRepository.AddAsync(participant);
        await unitOfWork.Commit();

        return participant;
    }

    private async Task<Participant?> GetParticipantById(Guid id)
    {
        using var scope = ServiceProvider?.CreateScope();

        if (scope is null)
            return null;

        var participantbRepository = scope.ServiceProvider.GetRequiredService<IParticipantRepository>();

        return await participantbRepository.GetByIdAsync(id);
    }
}