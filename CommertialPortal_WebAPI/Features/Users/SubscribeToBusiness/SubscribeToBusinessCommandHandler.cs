using CommertialPortal_WebAPI.Domain.Entities;
using CommertialPortal_WebAPI.Infrastructure.Data;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CommertialPortal_WebAPI.Features.Users.SubscribeToBusiness;

public class SubscribeToBusinessHandler : IRequestHandler<SubscribeToBusinessCommand, Result>
{
    private readonly DataContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SubscribeToBusinessHandler(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(SubscribeToBusinessCommand request, CancellationToken cancellationToken)
    {
        var email = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
    
        var user = await _dbContext.Users.Where(x => x.Email == email)
            .Include(x => x.ClientProfile)
            .FirstOrDefaultAsync();

        if (user is null || user.ClientProfile is null)
            return Result.Failure("User not found.");


        var clientProfile = await _dbContext.ClientProfiles
            .FirstOrDefaultAsync(cp => cp.Id == user.ClientProfile.Id, cancellationToken);

        if (clientProfile is null)
            return Result.Failure("Client profile not found.");

        var businessProfile = await _dbContext.BusinessProfiles
            .FirstOrDefaultAsync(bp => bp.Id == request.BusinessProfileId, cancellationToken);

        if (businessProfile is null)
            return Result.Failure("Business profile not found.");

        var alreadySubscribed = await _dbContext.ClientSubscriptions
            .AnyAsync(cs => cs.ClientProfileId == clientProfile.Id && cs.BusinessProfileId == businessProfile.Id, cancellationToken);

        if (alreadySubscribed)
            return Result.Failure("Already subscribed to this business.");

        var subscription = new ClientSubscription
        {
            ClientProfileId = clientProfile.Id,
            BusinessProfileId = businessProfile.Id
        };

        _dbContext.ClientSubscriptions.Add(subscription);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
