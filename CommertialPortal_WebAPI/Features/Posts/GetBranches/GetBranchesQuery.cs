using MediatR;

namespace CommertialPortal_WebAPI.Features.Posts.GetBranches;

public class GetBranchesQuery : IRequest<List<BranchDto>>
{
}
