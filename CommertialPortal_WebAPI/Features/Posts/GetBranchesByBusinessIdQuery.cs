using CommertialPortal_WebAPI.Features.Posts.GetBranches;
using MediatR;
using System.Collections.Generic;

namespace CommertialPortal_WebAPI.Features.Posts.GetBranchesByBusinessId;

public class GetBranchesByBusinessIdQuery : IRequest<List<BranchDto>>
{
    public int BusinessId { get; set; }

    public GetBranchesByBusinessIdQuery(int businessId)
    {
        BusinessId = businessId;
    }
}
