namespace CommertialPortal_WebAPI.Domain.Entities;

public class BusinessBranch
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public int BusinessProfileId { get; set; }
    public BusinessProfile BusinessProfile { get; set; } = null!;

    public List<PostBusinessBranch> PostBranches { get; set; } = new();
}
