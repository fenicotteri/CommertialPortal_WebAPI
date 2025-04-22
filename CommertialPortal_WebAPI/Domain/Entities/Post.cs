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

    public static Post Create(
        string title,
        string content,
        PostType type,
        string? imageUrl,
        DateTime startDate,
        DateTime? endDate,
        int businessProfileId,
        List<int> branchIds,
        DiscountInfo? discount = null)
    {
        var post = new Post
        {
            Title = title,
            Content = content,
            Type = type,
            ImageUrl = imageUrl,
            StartDate = startDate.ToUniversalTime(),
            EndDate = endDate?.ToUniversalTime(),
            CreatedAt = DateTime.UtcNow,
            BusinessProfileId = businessProfileId,
            Discount = discount
        };

        post.PostBranches = branchIds
        .Select(branchId => new PostBusinessBranch
        {
            BusinessBranchId = branchId,
            Post = post 
        })
        .ToList();

        return post;
    }
}


public class DiscountInfo
{
    public int Id { get; set; }
    public double? Percentage { get; set; }
    public decimal? Amount { get; set; }
    public string? Code { get; set; }

    public static DiscountInfo Create(double? percentage, decimal? amount, string? code)
    {
        return new DiscountInfo
        {
            Percentage = percentage,
            Amount = amount,
            Code = code
        };
    }
}


public enum PostType
{
    Event,
    Promotion,
    Discount
}

