using CommertialPortal_WebAPI.Application.Interfaces;
using MimeKit;
using Microsoft.Extensions.Options;

namespace CommertialPortal_WebAPI.Infrastructure.Servises;

internal sealed class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;

    public EmailService(IOptions<EmailOptions> emailOptions)
    {
        _emailOptions = emailOptions.Value;
    }

    public async Task SendInvitationAcceptedEmailAsync(EmailData emailData, CancellationToken cancellationToken = default)
    {

        try
        {

            MimeMessage emailMessage = new();
            MailboxAddress emailFrom = new(_emailOptions.Name, _emailOptions.EmailId);
            emailMessage.From.Add(emailFrom);

            MailboxAddress emailTo = new(emailData.EmailToName, emailData.EmailToId);
            emailMessage.To.Add(emailTo);

            emailMessage.Subject = emailData.EmailSubject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailData.EmailBody
            };

            using (var emailClient = new MailKit.Net.Smtp.SmtpClient())
            {
                await emailClient.ConnectAsync(_emailOptions.Host, _emailOptions.Port, _emailOptions.UseSSL, cancellationToken);
                await emailClient.AuthenticateAsync(_emailOptions.EmailId, _emailOptions.Password, cancellationToken);
                await emailClient.SendAsync(emailMessage, cancellationToken);
                await emailClient.DisconnectAsync(true, cancellationToken);
            }
        }
        catch
        {
            throw new Exception("Something went wrong while sending the email.");
        }
    }

}