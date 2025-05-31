using CommertialPortal_WebAPI.Features.Users.GetBusinesses;
using CommertialPortal_WebAPI.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommertialPortal_WebAPI.Features.Users.GetBusinessById;

public class GetBusinessByIdQueryHandler : IRequestHandler<GetBusinessByIdQuery, BusinessProfileDto?>
{
    private readonly DataContext _dbContext;

    public GetBusinessByIdQueryHandler(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BusinessProfileDto?> Handle(GetBusinessByIdQuery request, CancellationToken cancellationToken)
    {
        var business = await _dbContext.BusinessProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (business == null)
            return null;

        return new BusinessProfileDto
        {
            Id = business.Id,
            CompanyName = business.CompanyName
        };
    }
}
