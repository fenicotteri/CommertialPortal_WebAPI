namespace CommertialPortal_WebAPI.Domain.Entities;

public class Post
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public PostType Type { get; set; }

    public string? ImageUrl { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; set; }

    public int? BusinessProfileId { get; set; }
    public BusinessProfile BusinessProfile { get; set; } = null!;
    public List<PostBusinessBranch> PostBranches { get; set; } = new();

    public DiscountInfo? Discount { get; set; }
}

public class DiscountInfo
{
    public int Id { get; set; }
    public double? Percentage { get; set; }
    public decimal? Amount { get; set; }
    public string? Code { get; set; }
}


public enum PostType
{
    Event,
    Promotion,
    Discount
}

