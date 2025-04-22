using CommertialPortal_WebAPI.Features.Users.RegisterBusiness;
using CSharpFunctionalExtensions;
using MediatR;

namespace CommertialPortal_WebAPI.Features.Users.RegisterClient;

public record RegisterClientCommand(string Email, string Password, string FirstName, string LastName) : IRequest<Result<RegisterClientResponse>>;
