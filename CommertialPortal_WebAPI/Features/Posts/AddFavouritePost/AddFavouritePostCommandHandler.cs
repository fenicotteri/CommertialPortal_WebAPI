using CommertialPortal_WebAPI.Domain.Entities;
using CommertialPortal_WebAPI.Infrastructure.Data;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace CommertialPortal_WebAPI.Features.Posts.AddFavouritePost;

public class AddFavouritePostCommandHandler : IRequestHandler<AddFavouritePostCommand, Result>
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddFavouritePostCommandHandler(DataContext context, IHttpContextAccessor contextAccessor)
    {
        _context = context;
        _httpContextAccessor = contextAccessor;
    }

    public async Task<Result> Handle(AddFavouritePostCommand request, CancellationToken cancellationToken)
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
            return Result.Failure("Post not found.");

        bool isAlreadyFavourite = await _context.Entry(user.ClientProfile)
            .Collection(cp => cp.FavouritePosts)
            .Query()
            .AnyAsync(p => p.Id == post.Id, cancellationToken);

        if (isAlreadyFavourite)
            return Result.Failure("This post is already favourite.");

        user.ClientProfile.FavouritePosts.Add(post);

        bool isSubscribed = await _context.ClientSubscriptions
            .AnyAsync(cs =>
            cs.ClientProfileId == user.ClientProfile.Id &&
            cs.BusinessProfileId == post.BusinessProfileId,
            cancellationToken);

        if (isSubscribed)
            post.Analitics.SubscriberLikes++;
        else post.Analitics.GuestLikes++;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}