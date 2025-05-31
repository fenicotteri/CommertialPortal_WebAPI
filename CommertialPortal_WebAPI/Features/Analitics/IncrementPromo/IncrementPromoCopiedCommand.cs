using CSharpFunctionalExtensions;
using MediatR;

namespace CommertialPortal_WebAPI.Features.Analitics.IncrementPromo;

public record IncrementPromoCopiedCommand(int PostId) : IRequest<Result>;

