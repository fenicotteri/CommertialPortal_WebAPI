using CSharpFunctionalExtensions;
using MediatR;

namespace CommertialPortal_WebAPI.Features.Posts.RemoveFavouritePost;

public record RemoveFavouritePostCommand(int PostId) : IRequest<Result>;
