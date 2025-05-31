using CommertialPortal_WebAPI.Infrastructure.Data;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace CommertialPortal_WebAPI.Features.Posts.RemoveFavouritePost;

public class RemoveFavouritePostCommandHandler : IRequestHandler<RemoveFavouritePostCommand, Result>
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RemoveFavouritePostCommandHandler(DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(RemoveFavouritePostCommand request, CancellationToken cancellationToken)
    {
        var email = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email))
            return Result.Failure("User email not found.");

        var user = await _context.Users
            .Where(x => x.Email == email)
            .Include(x => x.ClientProfile)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null || user.ClientProfile is null)
            return Result.Failure("Client profile not found.");


        var post = await _context.Posts
           .Where(x => x.Id == request.PostId)
           .Include(x => x.Analitics)
           .FirstOrDefaultAsync(cancellationToken);

        if (post is null)
            return Result.Failure("This post is not favourite.");

        bool isSubscribed = await _context.ClientSubscriptions
                .AnyAsync(cs =>
                    cs.ClientProfileId == user.ClientProfile.Id &&
                    cs.BusinessProfileId == post.BusinessProfileId,
                    cancellationToken);

        if (isSubscribed)
            post.Analitics.SubscriberLikes--;
        else
            post.Analitics.GuestLikes--;

        user.ClientProfile.FavouritePosts.Remove(post);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

