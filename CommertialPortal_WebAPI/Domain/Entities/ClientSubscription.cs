namespace CommertialPortal_WebAPI.Domain.Entities;

public class ClientSubscription
{
    public int Id { get; set; }

    public int ClientProfileId { get; set; }
    public ClientProfile ClientProfile { get; set; } = null!;

    public int BusinessProfileId { get; set; }
    public BusinessProfile BusinessProfile { get; set; } = null!;
}
