using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ScheduleBarbecue.Tests.Helpers;
using ScheduleBarbecue.Tests.Helpers.Features;
using ScheduleBarbecue.Application.Base.Models;
using ScheduleBarbecue.Application.Base.Persistence;
using ScheduleBarbecue.Application.Features.Barbecues;
using ScheduleBarbecue.Application.Features.Barbecues.Repositories;
using ScheduleBarbecue.Application.Features.Contributions;
using ScheduleBarbecue.Application.Features.Contributions.Repositories;
using ScheduleBarbecue.Application.Features.Contributions.Responses;
using ScheduleBarbecue.Application.Features.Participants;
using ScheduleBarbecue.Application.Features.Participants.Repositories;
using System.Net;
using System.Net.Http.Json;

namespace ScheduleBarbecue.Tests.Api.Controllers.v1;

public class ContributionControllerTests : IntegrationTestBaseWithFakeJWT<Program>
{
    private readonly HttpClient _client;

    public ContributionControllerTests()
    {
        _client = GetTestAppClient();
    }

    [Fact(DisplayName = "Should return status 'Unprocessable Entity' when create a contribution with a barbecue invalid")]
    public async Task CreateContribution_CreateAContributionInValidWithoutBarbecue_ShouldReturnStatusInValid()
    {
        //Arrange
        var participant = await CreateParticipant();

        var contribution = ContributionHelper.NewCreateContributionRequest();
        contribution.ParticipantId = participant.Id;

        //Act
        var response = await _client.PostAsJsonAsync("/v1/contribution/", contribution);

        //Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();

        var contributionResponse = JsonConvert.DeserializeObject<ResponseErrors>(content);
        contributionResponse.Should().NotBeNull();
        contributionResponse?.Notifications.Should().HaveCount(1);
        contributionResponse?.Notifications.Select(n => n.Code).Should().BeEquivalentTo(StatusCodes.Status422UnprocessableEntity.ToString());
        contributionResponse?.Notifications.Select(n => n.Description).Should().BeEquivalentTo("Churrasco não localizado.");
    }

    [Fact(DisplayName = "Should return status 'Unprocessable Entity' when create a contribution with a participant invalid")]
    public async Task CreateContribution_CreateAContributionInValidWithoutParticipant_ShouldReturnStatusInValid()
    {
        //Arrange
        var barbecue = await CreateBarbecue();

        var contribution = ContributionHelper.NewCreateContributionRequest();
        contribution.BarbecueId = barbecue.Id;

        //Act
        var response = await _client.PostAsJsonAsync("/v1/contribution/", contribution);

        //Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();

        var contributionResponse = JsonConvert.DeserializeObject<ResponseErrors>(content);
        contributionResponse.Should().NotBeNull();
        contributionResponse?.Notifications.Should().HaveCount(1);
        contributionResponse?.Notifications.Select(n => n.Code).Should().BeEquivalentTo(StatusCodes.Status422UnprocessableEntity.ToString());
        contributionResponse?.Notifications.Select(n => n.Description).Should().BeEquivalentTo("Participante não localizado.");
    }

    [Fact(DisplayName = "Should return status 'Bad Request' when create a contribution and value with drink is invalid")]
    public async Task CreateContribution_CreateAContributionInValidValueWithDrink_ShouldReturnStatusInalid()
    {
        //Arrange
        var barbecue = await CreateBarbecue();
        var participant = await CreateParticipant();

        var contribution = ContributionHelper.NewCreateContributionRequest();
        contribution.BarbecueId = barbecue.Id;
        contribution.ParticipantId = participant.Id;
        contribution.Value = 10m;

        //Act
        var response = await _client.PostAsJsonAsync("/v1/contribution/", contribution);

        //Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();

        var contributionResponse = JsonConvert.DeserializeObject<ResponseErrors>(content);
        contributionResponse.Should().NotBeNull();
        contributionResponse?.Notifications.Should().HaveCount(1);
        contributionResponse?.Notifications.Select(n => n.Code).Should().BeEquivalentTo(StatusCodes.Status400BadRequest.ToString());
        contributionResponse?.Notifications.Select(n => n.Description).Should().BeEquivalentTo($"Valor da contribuição com bebida informado deve ser igual ou maior que {string.Format("{0:C}", barbecue.SuggestedValueWithDrink)}.");
    }

    [Fact(DisplayName = "Should return status 'Bad Request' when create a contribution and value without drink is invalid")]
    public async Task CreateContribution_CreateAContributionInValidValueWithoutDrink_ShouldReturnStatusInalid()
    {
        //Arrange
        var barbecue = await CreateBarbecue();
        var participant = await CreateParticipant();

        var contribution = ContributionHelper.NewCreateContributionRequest();
        contribution.BarbecueId = barbecue.Id;
        contribution.ParticipantId = participant.Id;
        contribution.Value = 5m;
        contribution.WithDrink = false;

        //Act
        var response = await _client.PostAsJsonAsync("/v1/contribution/", contribution);

        //Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();

        var contributionResponse = JsonConvert.DeserializeObject<ResponseErrors>(content);
        contributionResponse.Should().NotBeNull();
        contributionResponse?.Notifications.Should().HaveCount(1);
        contributionResponse?.Notifications.Select(n => n.Code).Should().BeEquivalentTo(StatusCodes.Status400BadRequest.ToString());
        contributionResponse?.Notifications.Select(n => n.Description).Should().BeEquivalentTo($"Valor mínimo da contribuição informado deve ser igual ou maior que {string.Format("{0:C}", barbecue.SuggestedValueWithoutDrink)}.");
    }

    [Fact(DisplayName = "Should return status 'Created' when create a contribution valid")]
    public async Task CreateContribution_CreateAContributionValid_ShouldReturnStatusCreated()
    {
        //Arrange
        var barbecue = await CreateBarbecue();
        var participant = await CreateParticipant();

        var contribution = ContributionHelper.NewCreateContributionRequest();
        contribution.BarbecueId = barbecue.Id;
        contribution.ParticipantId = participant.Id;

        //Act
        var response = await _client.PostAsJsonAsync("/v1/contribution/", contribution);

        //Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();

        var contributionResponse = JsonConvert.DeserializeObject<ContributionResponse>(content);
        contributionResponse.Should().NotBeNull();

        var contributionDb = await GetContributionById(contributionResponse.Id);
        contributionDb.Should().NotBeNull();

        contributionResponse?.Id.Should().Be(contributionDb.Id);
        contributionResponse?.BarbecueId.Should().Be(contributionDb.BarbecueId);
        contributionResponse?.ParticipantId.Should().Be(contributionDb.ParticipantId);
        contributionResponse?.Value.Should().Be(contributionDb.Value);
    }

    [Fact(DisplayName = "Should return status 'Ok' when get all participant by barbecue id from contribution valid")]
    public async Task GetAllParticipantByBarbecueIdCompendious_GetAllParticipantByBarbecueIdValid_ShouldReturnStatusIsValid()
    {
        //Arrange
        var contribution = await CreateContribution();

        //Act
        var response = await _client.GetAsync($"/v1/contribution/getAllParticipantByBarbecueId/{contribution.BarbecueId}");

        //Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();

        var contributionParticipantResponse = JsonConvert.DeserializeObject<IEnumerable<ContributionParticipantResponse>>(content);

        contributionParticipantResponse.Should().NotBeNull();
        contributionParticipantResponse.Should().HaveCount(1);
        contributionParticipantResponse?.Select(x => x.Id).Should().Equal(contribution.Id);
        contributionParticipantResponse?.Select(x => x.Name).Should().BeEquivalentTo("Teste Participante");
        contributionParticipantResponse?.Select(x => x.Value).Should().Equal(contribution.Value);
    }

    [Fact(DisplayName = "Should return status 'No Content' when remove a contribution valid")]
    public async Task RemoveContribution_RemoveAContributionValid_ShouldReturnStatusNoContext()
    {
        //Arrange
        var contribution = await CreateContribution();

        //Act
        var response = await _client.DeleteAsync($"/v1/contribution/{contribution.Id}");

        //Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var contributionDb = await GetContributionById(contribution.Id);
        contributionDb.Should().BeNull();
    }

    private async Task<Barbecue?> CreateBarbecue()
    {
        using var scope = ServiceProvider?.CreateScope();

        if (scope is null)
            return null;

        var barbecue = BarbecueHelper.NewBarbecue();
        var barbecueRepository = scope.ServiceProvider.GetRequiredService<IBarbecueRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        await barbecueRepository.AddAsync(barbecue);
        await unitOfWork.Commit();

        return barbecue;
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

    private async Task<Contribution?> CreateContribution()
    {
        using var scope = ServiceProvider?.CreateScope();

        if (scope is null)
            return null;

        var barbecue = await CreateBarbecue();
        var participant = await CreateParticipant();

        var contribution = new Contribution(barbecue.Id, participant.Id, 20m);

        var contributionRepository = scope.ServiceProvider.GetRequiredService<IContributionRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        await contributionRepository.AddAsync(contribution);
        await unitOfWork.Commit();

        return contribution;
    }

    private async Task<Contribution?> GetContributionById(Guid id)
    {
        using var scope = ServiceProvider?.CreateScope();

        if (scope is null)
            return null;

        var contributionRepository = scope.ServiceProvider.GetRequiredService<IContributionRepository>();

        return await contributionRepository.GetByIdAsync(id);
    }
}