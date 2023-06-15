using ScheduleBarbecue.Application.Features.Users;

namespace ScheduleBarbecue.Application.Features.Tokens.Services.Contracts;

public interface ITokenService
{
    string GenerateToken(User user);
}