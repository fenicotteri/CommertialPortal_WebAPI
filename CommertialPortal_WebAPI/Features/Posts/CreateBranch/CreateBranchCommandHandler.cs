using CommertialPortal_WebAPI.Domain.Entities;
using CommertialPortal_WebAPI.Infrastructure.Data;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CommertialPortal_WebAPI.Features.Posts.CreateBranch;

public sealed class CreateBranchCommandHandler : IRequestHandler<CreateBranchCommand, Result<int>>
{
    private readonly UserManager<User> _userManager;
    private readonly DataContext _dataContext;

    public CreateBranchCommandHandler(UserManager<User> userManager, DataContext dataContext)
    {
        _userManager = userManager;
        _dataContext = dataContext;
    }

    public async Task<Result<int>> Handle(CreateBranchCommand command, CancellationToken cancellationToken)
    {
        var branch = new BusinessBranch 
        { 
            Location = command.Request.Location,
            Description = command.Request.Description,
        };

        var business = await _userManager.Users
            .Where(u => u.Email == command.Email)
            .Include(u => u.BusinessProfile)
            .FirstOrDefaultAsync();

        if (business is null) return Result.Failure<int>("");

        branch.BusinessProfileId = business.BusinessProfile!.Id;

        await _dataContext.BusinessBranches.AddAsync(branch);
        await _dataContext.SaveChangesAsync();

        return branch.Id;
    }
}
