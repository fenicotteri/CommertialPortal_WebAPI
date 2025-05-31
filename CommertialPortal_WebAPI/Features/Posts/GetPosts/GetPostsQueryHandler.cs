namespace CommertialPortal_WebAPI.Features.Posts.GetPosts;

using CommertialPortal_WebAPI.Infrastructure.Data;
using CommertialPortal_WebAPI.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;


public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, List<PostDto>>
{
    private readonly DataContext _context;

    public GetPostsQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<List<PostDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.Posts
            .Include(p => p.Discount)
            .Include(p => p.PostBranches)
                .ThenInclude(pb => pb.BusinessBranch)
            .ToListAsync(cancellationToken);

        return posts.Select(p => new PostDto
        {
            Id = p.Id,
            BusinessId = p.BusinessProfileId,
            Title = p.Title,
            Content = p.Content,
            Type = p.Type.ToString().ToLower(), 
            ImageUrl = p.ImageUrl,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            Location = p.PostBranches.FirstOrDefault()?.BusinessBranch?.Location, // Берём первую привязанную ветку
            Discount = p.Discount != null ? new PostDto.DiscountDto
            {
                Percentage = p.Discount.Percentage,
                Amount = p.Discount.Amount,
                Code = p.Discount.Code
            } : null
        }).ToList();
    }
}

