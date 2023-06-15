using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace ScheduleBarbecue.Tests.Helpers.Api;

public class FakeUserHelper : IAsyncActionFilter
{
    public readonly List<Claim> Claims;

    public FakeUserHelper(List<Claim> claims)
    {
        Claims = claims;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(Claims));
        await next();
    }
}