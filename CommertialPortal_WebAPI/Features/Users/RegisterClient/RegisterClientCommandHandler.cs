using CommertialPortal_WebAPI.Application.Interfaces;
using CommertialPortal_WebAPI.Domain.Entities;
using CommertialPortal_WebAPI.Features.Users.RegisterBusiness;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CommertialPortal_WebAPI.Features.Users.RegisterClient;

public class RegisterClientCommandHandler : IRequestHandler<RegisterClientCommand, Result<RegisterClientResponse>>
{
    private readonly ITokenService _tokenServise;
    private readonly UserManager<User> _userManager;

    public RegisterClientCommandHandler(UserManager<User> userManager, ITokenService tokenServise)
    {
        _userManager = userManager;
        _tokenServise = tokenServise;
    }

    public async Task<Result<RegisterClientResponse>> Handle(RegisterClientCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _userManager.FindByEmailAsync(request.Email);
        if (userExists is not null)
            return Result.Failure<RegisterClientResponse>(DomainErrors.User.EmailAlreadyInUse);

        var userResult = User.Create(request.Email);
        if (userResult.IsFailure)
            return Result.Failure<RegisterClientResponse>(userResult.Error);

        var user = userResult.Value;

        var profileResult = user.AddClientProfile(request.FirstName, request.LastName);
        if (profileResult.IsFailure)
            return Result.Failure<RegisterClientResponse>(profileResult.Error);

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
            return Result.Failure<RegisterClientResponse>($"User creation failed: {errors}");
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "Client");
        if (!roleResult.Succeeded)
        {
            var errors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
            return Result.Failure<RegisterClientResponse>($"Role assignment failed: {errors}");
        }

        string token = _tokenServise.CreateToken(user);

        return Result.Success(new RegisterClientResponse(token));
    }
}
