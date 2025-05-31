using CommertialPortal_WebAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using NuGet.Protocol.Plugins;
using MediatR;
using CommertialPortal_WebAPI.Infrastructure.Data;
using CommertialPortal_WebAPI.Features.Posts.NewPostCreatedEvent;

namespace CommertialPortal_WebAPI.Features.Posts.CreatePost;

public sealed class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, int>
{
    private readonly DataContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMediator _mediator;

    public CreatePostCommandHandler(DataContext dbContext, IHttpContextAccessor httpContextAccessor, IMediator mediator)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _mediator = mediator;
    }
    public async Task<int> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var email = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
        if (email is null)
            throw new UnauthorizedAccessException("User email not found.");

        var businessProfile = await _dbContext.BusinessProfiles
            .Include(bp => bp.User)
            .FirstOrDefaultAsync(bp => bp.User.Email == email, cancellationToken);

        if (businessProfile is null)
            throw new UnauthorizedAccessException("Business profile not found.");

        DiscountInfo? discount = null;
        if (request.Discount != null)
        {
            discount = DiscountInfo.Create(
                request.Discount.Percentage,
                request.Discount.Amount,
                request.Discount.Code
            );
        }

        var post = Post.Create(
            title: request.Title,
            content: request.Content,
            type: request.Type,
            imageUrl: request.ImageUrl,
            startDate: request.StartDate,
            endDate: request.EndDate,
            businessProfileId: businessProfile.Id,
            branchIds: request.BranchIds,
            discount: discount
        );

        post.BusinessProfile = businessProfile;
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new PostCreatedEvent(post.Id, businessProfile.Id), cancellationToken);

        return post.Id;
    }

}
