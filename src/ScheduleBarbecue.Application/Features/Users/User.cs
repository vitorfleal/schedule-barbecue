using Microsoft.AspNetCore.Identity;

namespace ScheduleBarbecue.Application.Features.Users;

public class User : IdentityUser<Guid>
{
    public string? Name { get; set; }
}