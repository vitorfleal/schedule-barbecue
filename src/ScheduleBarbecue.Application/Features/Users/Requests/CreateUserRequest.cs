﻿namespace ScheduleBarbecue.Application.Features.Users.Requests;

public class CreateUserRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}