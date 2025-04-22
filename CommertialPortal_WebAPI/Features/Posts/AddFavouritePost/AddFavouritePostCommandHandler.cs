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

        var user = await _context.Users.Where(x => x.Email == email).Include(x => x.ClientProfile).FirstOrDefaultAsync();

        if (user is null || user.ClientProfile is null)
            throw new Exception("Client profile not found.");

        var post = await _context.Posts.Where(x => x.Id == request.PostId).FirstOrDefaultAsync();

        if (post is null)
            throw new Exception("Post not found.");

        if (!user.ClientProfile.FavouritePosts.Any(p => p.Id == post.Id))
        {
            user.ClientProfile.FavouritePosts.Add(post);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            return Result.Failure("This post is already favourite");
        }

        return Result.Success();
    }
}
