using CSharpFunctionalExtensions;
using MediatR;

namespace CommertialPortal_WebAPI.Features.Posts.AddFavouritePost;
public record AddFavouritePostCommand(int PostId) : IRequest<Result>;
