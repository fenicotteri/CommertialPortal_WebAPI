using MediatR;

namespace CommertialPortal_WebAPI.Features.Posts.NewPostCreatedEvent;

public class PostCreatedEvent : INotification
{
    public int PostId { get; }
    public int BusinessId { get; }

    public PostCreatedEvent(int postId, int businessId)
    {
        PostId = postId;
        BusinessId = businessId;
    }
}
