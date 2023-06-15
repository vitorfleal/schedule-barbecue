using ScheduleBarbecue.Application.Base.Models;
using ScheduleBarbecue.Application.Features.Users.Requests;

namespace ScheduleBarbecue.Application.Features.Users.Services.Contracts;

public interface IUserService
{
    Task<(Response, User?)> CreateUser(CreateUserRequest request, CancellationToken cancellationToken = default);

    Task<(Response, string?)> GetToken(UserLoginRequest request, CancellationToken cancellationToken = default);
}