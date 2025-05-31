using CommertialPortal_WebAPI.Infrastructure.Data;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CommertialPortal_WebAPI.Features.Analitics.IncrementView;

public class IncrementViewCountCommandHandler : IRequestHandler<IncrementViewCountCommand, Result>
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IncrementViewCountCommandHandler(DataContext context, IHttpContextAccessor accessor)
    {
        _context = context;
        _httpContextAccessor = accessor;
    }

    public async Task<Result> Handle(IncrementViewCountCommand request, CancellationToken cancellationToken)
    {
        var post = await _context.Posts
            .Where(p => p.Id == request.PostId)
            .Include(p => p.Analitics)
            .FirstOrDefaultAsync(cancellationToken);

        if (post is null)
            return Result.Failure("Post not found.");

        var email = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;

        if (email != null)
        {
            var clientProfileId = await _context.ClientProfiles
                .Where(cp => cp.User.Email == email)
                .Select(cp => cp.Id)
                .FirstOrDefaultAsync(cancellationToken);

            bool isSubscribed = await _context.ClientSubscriptions
                .AnyAsync(cs => cs.ClientProfileId == clientProfileId && cs.BusinessProfileId == post.BusinessProfileId, cancellationToken);

            if (isSubscribed)
                post.Analitics.SubscriberViews++;
            else
                post.Analitics.GuestViews++;
        }
        else
        {
            post.Analitics.GuestViews++;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}

