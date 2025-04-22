using CommertialPortal_WebAPI.Application.Interfaces;
using CommertialPortal_WebAPI.Domain.Entities;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CommertialPortal_WebAPI.Features.Users.RegisterBusiness;

public class RegisterUserHandler : IRequestHandler<RegisterBusinessCommand, Result<RegisterBusinessResponse>>
{
    private readonly ITokenServise _tokenServise;
    private readonly UserManager<User> _userManager;

    public RegisterUserHandler(ITokenServise tokenServise, UserManager<User> userManager)
    {
        _tokenServise = tokenServise;
        _userManager = userManager;
    }

    public async Task<Result<RegisterBusinessResponse>> Handle(RegisterBusinessCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _userManager.FindByEmailAsync(request.Email);
        if (userExists is not null) 
            return Result.Failure<RegisterBusinessResponse>(DomainErrors.User.EmailAlreadyInUse);

        var userResult = User.Create(request.Email);
        if (userResult.IsFailure)
            return Result.Failure<RegisterBusinessResponse>(userResult.Error);

        var user = userResult.Value;

        var profileResult = user.AddBusinessProfile(request.companyName);
        if (profileResult.IsFailure)
            return Result.Failure<RegisterBusinessResponse>(profileResult.Error);

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
            return Result.Failure<RegisterBusinessResponse>($"User creation failed: {errors}");
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "Business");
        if (!roleResult.Succeeded)
        {
            var errors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
            return Result.Failure<RegisterBusinessResponse>($"Role assignment failed: {errors}");
        }

        string token = _tokenServise.CreateToken(user);

        return Result.Success(new RegisterBusinessResponse(token));
    }
}
