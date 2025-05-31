using System.Security.Claims;
using CommertialPortal_WebAPI.Domain.Entities;
using CommertialPortal_WebAPI.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace CommertialPortal_WebAPI.Features.Analitics.GetAnalitics;

public class GetBusinessAnalyticsQueryHandler : IRequestHandler<GetBusinessAnalyticsQuery, BusinessAnaliticsDto>
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetBusinessAnalyticsQueryHandler(DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BusinessAnaliticsDto> Handle(GetBusinessAnalyticsQuery request, CancellationToken cancellationToken)
    {
        var email = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(email))
            throw new Exception("Email not found.");

        var user = await _context.Users
            .Include(u => u.BusinessProfile)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (user?.BusinessProfile == null)
            throw new Exception("Business profile not found.");

        var businessId = user.BusinessProfile.Id;

        var posts = await _context.Posts
            .Where(p => p.BusinessProfileId == businessId)
            .Include(p => p.Analitics)
            .ToListAsync(cancellationToken);

        var postAnalyticsDtos = posts
            .Where(p => p.Analitics != null)
            .Select(p => new PostAnaliticsDto(
                Title: p.Title,
                Type: p.Type,
                GuestLikes: p.Analitics!.GuestLikes,
                SubscriberLikes: p.Analitics.SubscriberLikes,
                GuestViews: p.Analitics.GuestViews,
                SubscriberViews: p.Analitics.SubscriberViews,
                PromosCopied: p.Analitics.PromosCopied ?? 0
            )).ToList();

        var totalGuestLikes = postAnalyticsDtos.Sum(p => p.GuestLikes);
        var totalSubscriberLikes = postAnalyticsDtos.Sum(p => p.SubscriberLikes);
        var totalGuestViews = postAnalyticsDtos.Sum(p => p.GuestViews);
        var totalSubscriberViews = postAnalyticsDtos.Sum(p => p.SubscriberViews);
        var totalPromosCopied = postAnalyticsDtos.Sum(p => p.PromosCopied);

        var subscribersCount = await _context.ClientSubscriptions
            .CountAsync(s => s.BusinessProfileId == businessId, cancellationToken);

        return new BusinessAnaliticsDto
        {
            TotalGuestLikes = totalGuestLikes,
            TotalSubscriberLikes = totalSubscriberLikes,
            TotalGuestViews = totalGuestViews,
            TotalSubscriberViews = totalSubscriberViews,
            TotalPromosCopied = totalPromosCopied,
            TotalLikes = totalGuestLikes + totalSubscriberLikes,
            TotalViews = totalGuestViews + totalSubscriberViews,
            SubscribersCount = subscribersCount,
            PostAnalitics = postAnalyticsDtos
        };
    }
}