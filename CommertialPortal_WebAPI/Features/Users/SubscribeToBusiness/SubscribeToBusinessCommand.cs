using CSharpFunctionalExtensions;
using MediatR;

namespace CommertialPortal_WebAPI.Features.Users.SubscribeToBusiness;

public record SubscribeToBusinessCommand(int BusinessProfileId) : IRequest<Result>;
