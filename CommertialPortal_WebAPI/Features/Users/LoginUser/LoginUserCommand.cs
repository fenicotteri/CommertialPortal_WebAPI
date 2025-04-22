namespace CommertialPortal_WebAPI.Features.Users.LoginUser;

using CSharpFunctionalExtensions;
using MediatR;

public record LoginUserCommand(string Email, string Password) : IRequest<Result<LoginUserResponse>>;

public record LoginUserResponse(string AccessToken);

