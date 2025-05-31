using System.Security.Claims;
using CommertialPortal_WebAPI.Features.Posts.GetPosts;
using CommertialPortal_WebAPI.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http.Logging;

namespace CommertialPortal_WebAPI.Features.Posts.GetFavouritePosts;

public class GetFavouritePostsQuerryHandler : IRequestHandler<GetFavouritePostsQuerry, List<int>>
{
    private readonly DataContext _context;
    private IHttpContextAccessor _httpContextAccessor;

    public GetFavouritePostsQuerryHandler(DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<int>> Handle(GetFavouritePostsQuerry request, CancellationToken cancellationToken)
    {
        var email = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
        if (email is null)
            throw new UnauthorizedAccessException("User email not found.");

        var clientProfile = await _context.ClientProfiles
            .Include(bp => bp.User)
            .FirstOrDefaultAsync(bp => bp.User.Email == email, cancellationToken);

        if (clientProfile is null)
            throw new UnauthorizedAccessException("Client profile not found.");

        var users = await _context.Users.Include(x => x.ClientProfile).ThenInclude(x => x.FavouritePosts).FirstOrDefaultAsync(x => x.Email == email);
        return users.ClientProfile.FavouritePosts.Select(x => x.Id).ToList();
        
    }
}
