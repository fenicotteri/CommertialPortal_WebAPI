namespace CommertialPortal_WebAPI.Features.Posts.NewPostCreatedEvent;

using AutoMapper.Execution;
using CommertialPortal_WebAPI.Application.Interfaces;
using CommertialPortal_WebAPI.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class PostCreatedEventHandler : INotificationHandler<PostCreatedEvent>
{
    private readonly DataContext _dbContext;
    private readonly IEmailService _emailService;

    public PostCreatedEventHandler(DataContext dbContext, IEmailService emailService)
    {
        _dbContext = dbContext;
        _emailService = emailService;
    }

    public async Task Handle(PostCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Находим всех подписанных пользователей на бизнес
        var subscribers = await _dbContext.ClientSubscriptions
            .Where(s => s.BusinessProfile.Id == notification.BusinessId)
            .Select(s => s.ClientProfile.User)
            .ToListAsync(cancellationToken);

        var post = await _dbContext.Posts.Where(x => x.Id == notification.PostId).FirstOrDefaultAsync();
        var business = await _dbContext.BusinessProfiles.Where(x => x.Id == notification.BusinessId).FirstOrDefaultAsync();

        // Отправляем уведомление каждому подписчику
        foreach (var subscriber in subscribers)
        {
            string FilePath = "C:/Users/mag20/source/repos/CommertialPortal_WebAPI/CommertialPortal_WebAPI/PostCreatedNotificationCard.html";
            string EmailTemplateText = File.ReadAllText(FilePath);

            EmailTemplateText = string.Format(EmailTemplateText,
                business.CompanyName,
                post.Title,
                post.Type,
                post.Title,
                post.Content);

            EmailData emailData = new(
                subscriber.Email,
                "Hi",
                "New post.",
                EmailTemplateText);

            await _emailService.SendInvitationAcceptedEmailAsync(emailData, cancellationToken);
        }
       
    }
}

