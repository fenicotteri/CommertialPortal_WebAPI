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

        var user = await _context.Users.Where(x => x.Email == email).Include(x => x.ClientProfile).FirstOrDefaultAsync();

        if (user is null || user.ClientProfile is null)
            throw new Exception("Client profile not found.");

        var post = user.ClientProfile.FavouritePosts.FirstOrDefault(p => p.Id == request.PostId);

        if (post is not null)
        {
            user.ClientProfile.FavouritePosts.Remove(post);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            return Result.Failure("This post is not favourite");
        }

        return Result.Success();
    }
}

