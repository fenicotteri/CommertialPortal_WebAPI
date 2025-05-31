using CommertialPortal_WebAPI.Domain.Entities;
using MediatR;

namespace CommertialPortal_WebAPI.Features.Posts.GetPosts;

public class GetPostsQuery : IRequest<List<PostDto>>
{
}

