namespace CommertialPortal_WebAPI.Features.Users.GetMeUser;

public class GetMeDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserType { get; set; } = string.Empty; 
    public int? ProfileId { get; set; }
}