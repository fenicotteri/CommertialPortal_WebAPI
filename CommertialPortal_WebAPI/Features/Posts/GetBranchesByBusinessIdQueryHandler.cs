using CommertialPortal_WebAPI.Features.Posts.GetBranches;
using CommertialPortal_WebAPI.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommertialPortal_WebAPI.Features.Posts.GetBranchesByBusinessId;

public class GetBranchesByBusinessIdQueryHandler : IRequestHandler<GetBranchesByBusinessIdQuery, List<BranchDto>>
{
    private readonly DataContext _context;

    public GetBranchesByBusinessIdQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<List<BranchDto>> Handle(GetBranchesByBusinessIdQuery request, CancellationToken cancellationToken)
    {
        var branches = await _context.BusinessBranches
            .Where(b => b.BusinessProfileId == request.BusinessId)
            .ToListAsync(cancellationToken);

        return branches.Select(branch => new BranchDto
        {
            Id = branch.Id,
            Description = branch.Description,
            Location = branch.Location,
        }).ToList();
    }
}
