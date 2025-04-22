namespace CommertialPortal_WebAPI.Domain.Entities;

public class PostBusinessBranch
{
    public int PostId { get; set; }
    public Post Post { get; set; } = null!;

    public int BusinessBranchId { get; set; }
    public BusinessBranch BusinessBranch { get; set; } = null!;
}
