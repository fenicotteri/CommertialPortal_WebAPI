using CommertialPortal_WebAPI.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CommertialPortal_WebAPI.Features.Users.GetBusinesses
{
    public class GetBusinessesQueryHandler : IRequestHandler<GetBusinessesQuery, List<BusinessProfileDto>>
    {
        private readonly DataContext _dbContext;

        public GetBusinessesQueryHandler(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<BusinessProfileDto>> Handle(GetBusinessesQuery request, CancellationToken cancellationToken)
        {
            var businesses = await _dbContext.BusinessProfiles
                .AsNoTracking()
                .Select(b => new BusinessProfileDto
                {
                    Id = b.Id,
                    CompanyName = b.CompanyName
                })
                .ToListAsync(cancellationToken);

            return businesses;
        }
    }
}
