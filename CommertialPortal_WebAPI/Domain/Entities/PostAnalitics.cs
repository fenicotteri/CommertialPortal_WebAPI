namespace CommertialPortal_WebAPI.Domain.Entities;

public class PostAnalitics
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int GuestLikes{ get; set; } = 0;

    public int SubscriberLikes { get; set; } = 0;
    public int GuestViews { get; set; } = 0;
    public int SubscriberViews { get; set; } = 0;
    public int? PromosCopied { get; set; } = 0;
}
