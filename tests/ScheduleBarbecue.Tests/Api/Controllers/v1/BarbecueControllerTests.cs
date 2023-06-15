using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ScheduleBarbecue.Tests.Helpers;
using ScheduleBarbecue.Tests.Helpers.Features;
using ScheduleBarbecue.Application.Base.Persistence;
using ScheduleBarbecue.Application.Features.Barbecues;
using ScheduleBarbecue.Application.Features.Barbecues.Repositories;
using ScheduleBarbecue.Application.Features.Barbecues.Responses;
using ScheduleBarbecue.Application.Features.Contributions;
using ScheduleBarbecue.Application.Features.Contributions.Repositories;
using ScheduleBarbecue.Application.Features.Participants;
using ScheduleBarbecue.Application.Features.Participants.Repositories;
using System.Net;
using System.Net.Http.Json;

namespace ScheduleBarbecue.Tests.Api.Controllers.v1;

public class BarbecueControllerTests : IntegrationTestBaseWithFakeJWT<Program>
{
    private readonly HttpClient _client;

    public BarbecueControllerTests()
    {
        _client = GetTestAppClient();
    }

    [Fact(DisplayName = "Should return status 'Created' when create a barbecue valid")]
    public async Task CreateBarbecue_CreateABarbecueValid_ShouldReturnStatusCreated()
    {
        //Arrange
        var barbecue = BarbecueHelper.NewCreateBarbecueRequest();

        //Act
        var response = await _client.PostAsJsonAsync("/v1/barbecue/", barbecue);

        //Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();

        var barbecueResponse = JsonConvert.DeserializeObject<BarbecueResponse>(content);
        barbecueResponse.Should().NotBeNull();

        var barbecueDb = await GetBarbecueById(barbecueResponse.Id);
        barbecueDb.Should().NotBeNull();

        barbecueResponse?.Id.Should().Be(barbecueDb.Id);
        barbecueResponse?.Name.Should().BeEquivalentTo(barbecueDb.Name);
        barbecueResponse?.Description.Should().BeEquivalentTo(barbecueDb.Description);
        barbecueResponse?.AdditionalNote.Should().BeEquivalentTo(barbecueDb.AdditionalNote);
        barbecueResponse?.SuggestedValueWithDrink.Should().Be(barbecueDb.SuggestedValueWithDrink);
        barbecueResponse?.SuggestedValueWithoutDrink.Should().Be(barbecueDb.SuggestedValueWithoutDrink);
        barbecueResponse?.HasSuggestedValue.Should().Be(barbecueDb.HasSuggestedValue);
    }

    [Fact(DisplayName = "Should return status 'Ok' when get all barbecue valid")]
    public async Task GetAllBarbecue_GetAllBarbecueValid_ShouldReturnStatusIsValid()
    {
        //Arrange
        var barbecue = await CreateBarbecue();

        //Act
        var response = await _client.GetAsync("/v1/barbecue");

        //Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();

        var barbecueResponse = JsonConvert.DeserializeObject<IEnumerable<BarbecueResponse>>(content);
        barbecueResponse.Should().NotBeNull();
        barbecueResponse.Should().HaveCount(1);
        barbecueResponse?.Select(x => x.Id).Should().Equal(barbecue.Id);
        barbecueResponse?.Select(x => x.Name).Should().BeEquivalentTo(barbecue?.Name);
        barbecueResponse?.Select(x => x.Description).Should().BeEquivalentTo(barbecue?.Description);
        barbecueResponse?.Select(x => x.AdditionalNote).Should().BeEquivalentTo(barbecue?.AdditionalNote);
        barbecueResponse?.Select(x => x.Date).Should().Equal(barbecue?.Date);
        barbecueResponse?.Select(x => x.SuggestedValueWithDrink).Should().Equal(barbecue.SuggestedValueWithDrink);
        barbecueResponse?.Select(x => x.SuggestedValueWithoutDrink).Should().Equal(barbecue.SuggestedValueWithoutDrink);
        barbecueResponse?.Select(x => x.HasSuggestedValue).Should().Equal(barbecue.HasSuggestedValue);
    }

    [Fact(DisplayName = "Should return status 'Ok' when get one barbecue compendious valid")]
    public async Task GetOneBarbecueCompendious_GetOneBarbecueCompendiousValid_ShouldReturnStatusIsValid()
    {
        //Arrange
        var contribution = await CreateContribution();

        //Act
        var response = await _client.GetAsync($"/v1/barbecue/getOneBarbecueCompendious/{contribution.BarbecueId}");

        //Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();

        var barbecueCompendiousResponse = JsonConvert.DeserializeObject<IEnumerable<BarbecueCompendiousResponse>>(content);
        barbecueCompendiousResponse.Should().NotBeNull();
        barbecueCompendiousResponse.Should().HaveCount(1);
        barbecueCompendiousResponse?.Select(x => x.Id).Should().Equal(contribution.BarbecueId);
        barbecueCompendiousResponse?.Select(x => x.Name).Should().BeEquivalentTo("Teste Barbecue");
        barbecueCompendiousResponse?.Select(x => x.TotalParticipants).Should().Equal(1);
        barbecueCompendiousResponse?.Select(x => x.ContributionAmount).Should().Equal(20m);
    }

    [Fact(DisplayName = "Should return status 'Ok' when get all barbecue compendious valid")]
    public async Task GetAllBarbecueCompendious_GetAllBarbecueCompendiousValid_ShouldReturnStatusIsValid()
    {
        //Arrange
        var contribution = await CreateContribution();

        //Act
        var response = await _client.GetAsync("/v1/barbecue/getAllBarbecueCompendious");

        //Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();

        var barbecueCompendiousResponse = JsonConvert.DeserializeObject<IEnumerable<BarbecueCompendiousResponse>>(content);
        barbecueCompendiousResponse.Should().NotBeNull();
        barbecueCompendiousResponse.Should().HaveCount(1);
        barbecueCompendiousResponse?.Select(x => x.Id).Should().Equal(contribution.BarbecueId);
        barbecueCompendiousResponse?.Select(x => x.Name).Should().BeEquivalentTo("Teste Barbecue");
        barbecueCompendiousResponse?.Select(x => x.TotalParticipants).Should().Equal(1);
        barbecueCompendiousResponse?.Select(x => x.ContributionAmount).Should().Equal(20m);
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

    private async Task<Barbecue?> GetBarbecueById(Guid id)
    {
        using var scope = ServiceProvider?.CreateScope();

        if (scope is null)
            return null;

        var barbecueRepository = scope.ServiceProvider.GetRequiredService<IBarbecueRepository>();

        return await barbecueRepository.GetByIdAsync(id);
    }
}