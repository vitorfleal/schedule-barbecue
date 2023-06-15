using AutoMapper;
using ScheduleBarbecue.Application.Features.Barbecues;
using ScheduleBarbecue.Application.Features.Barbecues.DTOs;
using ScheduleBarbecue.Application.Features.Barbecues.Responses;
using ScheduleBarbecue.Application.Features.Contributions.DTOs;
using ScheduleBarbecue.Application.Features.Contributions.Responses;
using ScheduleBarbecue.Application.Features.Participants;
using ScheduleBarbecue.Application.Features.Participants.Responses;

namespace ScheduleBarbecue.Application.AutoMapper;

public class ConfiguringMapperProfile : Profile
{
    public ConfiguringMapperProfile()
    {
        CreateMap<BarbecueResponse, Barbecue>()
            .ReverseMap();

        CreateMap<BarbecueCompendiousResponse, BarbecueDto>()
           .ReverseMap();

        CreateMap<ContributionParticipantResponse, ContributionParticipantDto>()
           .ReverseMap();

        CreateMap<ParticipantResponse, Participant>()
            .ReverseMap();

        CreateMap<ParticipantContributionResponse, Participant>()
            .ReverseMap();
    }
}