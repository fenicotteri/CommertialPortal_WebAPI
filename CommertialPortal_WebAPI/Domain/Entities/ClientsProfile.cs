using Microsoft.Extensions.Hosting;

namespace CommertialPortal_WebAPI.Domain.Entities;

public class ClientProfile
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public User User { get; set; } = null!;
    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public ICollection<Post> FavouritePosts { get; set; } = new List<Post>();

    public List<ClientSubscription> Subscriptions { get; set; } = new();

    public static ClientProfile Create(string userId, string firstName, string lastName)
    {
        return new ClientProfile
        {
            UserId = userId,
            FirstName = firstName,
            LastName = lastName
        };
    }

}
