namespace CommertialPortal_WebAPI.Features.Posts.GetPosts;

public class PostDto
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Location { get; set; }

    public DiscountDto? Discount { get; set; }

    public class DiscountDto
    {
        public double? Percentage { get; set; }
        public decimal? Amount { get; set; }
        public string? Code { get; set; }
    }
}
