using CommertialPortal_WebAPI.Domain.Entities;

namespace CommertialPortal_WebAPI.Features.Analitics.GetAnalitics;

public class BusinessAnaliticsDto
{
    public int TotalGuestLikes { get; set; } 
    public int TotalSubscriberLikes { get; set; } 
    public int TotalGuestViews { get; set; } 
    public int TotalSubscriberViews { get; set; } 
    public int TotalPromosCopied { get; set; }
    public int TotalLikes { get; set; }
    public int TotalViews { get; set; } 
    public int SubscribersCount { get; set; }

    public List<PostAnaliticsDto> PostAnalitics { get; set; } = new List<PostAnaliticsDto>();
}

public record PostAnaliticsDto(string Title, PostType Type, int GuestLikes, int SubscriberLikes, int GuestViews, int SubscriberViews, int PromosCopied);
