using CommertialPortal_WebAPI.Domain.Entities;
using MediatR;

namespace CommertialPortal_WebAPI.Features.Posts.CreatePost;

public record CreatePostCommand(
    string Title,
    string Content,
    PostType Type,
    string? ImageUrl,
    DateTime StartDate,
    DateTime? EndDate,
    DiscountInfoDto? Discount,
    List<int> BranchIds
) : IRequest<int>;

public record DiscountInfoDto(
    double? Percentage,
    decimal? Amount,
    string? Code
);
