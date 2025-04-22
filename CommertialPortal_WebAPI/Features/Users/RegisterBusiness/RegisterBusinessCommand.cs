using CSharpFunctionalExtensions;
using MediatR;

namespace CommertialPortal_WebAPI.Features.Users.RegisterBusiness;

public record RegisterBusinessCommand(string Email, string Password, string companyName) : IRequest<Result<RegisterBusinessResponse>>;

