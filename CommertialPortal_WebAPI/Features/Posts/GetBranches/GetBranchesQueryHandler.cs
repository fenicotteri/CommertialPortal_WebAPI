using CommertialPortal_WebAPI.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommertialPortal_WebAPI.Features.Posts.GetBranches;

public class GetBranchesQueryHandler : IRequestHandler<GetBranchesQuery, List<BranchDto>>
{
    private readonly DataContext _context;

    public GetBranchesQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<List<BranchDto>> Handle(GetBranchesQuery request, CancellationToken cancellationToken)
    {
        return await _context.BusinessBranches
            .AsNoTracking()
            .Select(branch => new BranchDto
            {
                Id = branch.Id,
                Description = branch.Description,
                Location = branch.Location
            })
            .ToListAsync(cancellationToken);
    }
}
