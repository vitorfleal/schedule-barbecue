namespace ScheduleBarbecue.Application.Features.Users.Requests;

public class UserLoginRequest
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
}