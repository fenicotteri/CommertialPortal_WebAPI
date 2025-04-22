using CommertialPortal_WebAPI.Application.Interfaces;
using CommertialPortal_WebAPI.Domain.Entities;
using CommertialPortal_WebAPI.Infrastructure.Data;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CommertialPortal_WebAPI.Features.Users.LoginUser;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, Result<LoginUserResponse>>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenServise _tokenServise;
    private readonly DataContext _dataContext;

    public LoginUserHandler(UserManager<User> userManager, ITokenServise tokenServise, DataContext dataContext)
    {
        _userManager = userManager;
        _tokenServise = tokenServise;
        _dataContext = dataContext;
    }

    public async Task<Result<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dataContext.Users
            .Where(x => x.Email == request.Email)
            .Include(x => x.ClientProfile)
            .FirstOrDefaultAsync();

        if (user == null)
            return Result.Failure<LoginUserResponse>("There is no User with this email.");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
            return Result.Failure<LoginUserResponse>("Wrong password.");

        var token = _tokenServise.CreateToken(user);

        return Result.Success(new LoginUserResponse(token));
    }
}
