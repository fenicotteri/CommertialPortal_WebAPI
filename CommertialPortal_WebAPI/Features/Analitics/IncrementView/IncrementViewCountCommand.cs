using CSharpFunctionalExtensions;
using MediatR;

namespace CommertialPortal_WebAPI.Features.Analitics.IncrementView;

public record IncrementViewCountCommand(int PostId) : IRequest<Result>;

