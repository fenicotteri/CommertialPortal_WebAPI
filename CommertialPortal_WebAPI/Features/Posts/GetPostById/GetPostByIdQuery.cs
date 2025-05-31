using CommertialPortal_WebAPI.Features.Posts.GetPosts;
using MediatR;

namespace CommertialPortal_WebAPI.Features.Posts.GetPostById;


    public class GetPostByIdQuery : IRequest<PostDto?>
    {
        public int Id { get; }

        public GetPostByIdQuery(int id)
        {
            Id = id;
        }
    }


