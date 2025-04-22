using CSharpFunctionalExtensions;
using MediatR;

namespace CommertialPortal_WebAPI.Features.Posts.CreateBranch;

public record CreateBranchCommand(string? Email, CreateBrunchRequest Request) : IRequest<Result<int>>;

