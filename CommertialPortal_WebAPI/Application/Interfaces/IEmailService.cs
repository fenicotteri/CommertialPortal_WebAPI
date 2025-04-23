namespace CommertialPortal_WebAPI.Application.Interfaces;
public interface IEmailService
{
    Task SendInvitationAcceptedEmailAsync(EmailData emailData, CancellationToken cancellationToken = default);
}
