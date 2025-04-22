using Microsoft.Extensions.Hosting;

namespace CommertialPortal_WebAPI.Domain.Entities;

public class BusinessProfile
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public User User { get; set; } = null!;

    public string CompanyName { get; set; } = string.Empty;
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public List<BusinessBranch> Branches { get; set; } = new();
    public static BusinessProfile Create(string userId, string companyName)
    {
        return new BusinessProfile
        {
            UserId = userId,
            CompanyName = companyName,
        };
    }
}
