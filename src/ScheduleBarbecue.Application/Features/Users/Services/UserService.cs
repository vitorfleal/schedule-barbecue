using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ScheduleBarbecue.Application.Base.Models;
using ScheduleBarbecue.Application.Features.Tokens.Services.Contracts;
using ScheduleBarbecue.Application.Features.Users.Requests;
using ScheduleBarbecue.Application.Features.Users.Services.Contracts;

namespace ScheduleBarbecue.Application.Features.Users.Services;

public class UserService : IUserService
{
    private readonly ITokenService _tokenService;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public UserService(
        ITokenService tokenService,
        SignInManager<User> signInManager,
        UserManager<User> userManager)
    {
        _tokenService = tokenService;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<(Response, User?)> CreateUser(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = new User
        {
            Name = request.Name,
            UserName = request.Email,
            Email = request.Email,
        };

        try
        {
            var createResult = await _userManager.CreateAsync(user, request.Password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join("-", createResult.Errors.Select(t => t.Description));

                return (Response.Invalid(StatusCodes.Status422UnprocessableEntity.ToString(), errors), user);
            }

            return (Response.Valid(), user);
        }
        catch (Exception ex)
        {
            return (Response.Invalid(StatusCodes.Status422UnprocessableEntity.ToString(), ex.Message), user);
        }
    }

    public async Task<(Response, string?)> GetToken(UserLoginRequest request, CancellationToken cancellationToken = default)
    {
        var login = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);

        try
        {
            if (login.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(request.UserName);

                var token = _tokenService.GenerateToken(user);

                return (Response.Valid(), token);
            }
            else
                return (Response.Invalid(StatusCodes.Status404NotFound.ToString(), "E-mail e/ou senha inválidos."), string.Empty);
        }
        catch (Exception ex)
        {
            return (Response.Invalid(StatusCodes.Status422UnprocessableEntity.ToString(), ex.Message), null);
        }
    }
}