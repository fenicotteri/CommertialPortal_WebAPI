using CommertialPortal_WebAPI.Features.Posts.GetPosts;
using CommertialPortal_WebAPI.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommertialPortal_WebAPI.Features.Posts.GetPostById;


public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostDto?>
{
    private readonly DataContext _context;

    public GetPostByIdQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<PostDto?> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _context.Posts
            .Include(p => p.Discount)
            .Include(p => p.PostBranches)
                .ThenInclude(pb => pb.BusinessBranch)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (post == null)
        {
            return null;
        }

        return new PostDto
        {
            Id = post.Id,
            BusinessId = post.BusinessProfileId,
            Title = post.Title,
            Content = post.Content,
            Type = post.Type.ToString().ToLower(),
            ImageUrl = post.ImageUrl,
            StartDate = post.StartDate,
            EndDate = post.EndDate,
            Location = post.PostBranches.FirstOrDefault()?.BusinessBranch?.Location,
            Discount = post.Discount != null ? new PostDto.DiscountDto
            {
                Percentage = post.Discount.Percentage,
                Amount = post.Discount.Amount,
                Code = post.Discount.Code
            } : null
        };
    }
}

