using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;

namespace CommertialPortal_WebAPI.Domain.Entities;

public class User : IdentityUser
{
    public BusinessProfile? BusinessProfile { get; set; }
    public ClientProfile? ClientProfile { get; set; }

    public UserType? UserType { get; private set; }

    public static Result<User> Create(string email)
    {
        return Result.Success(new User
        {
            Email = email,
            UserName = email,
        });
    }

    public Result AddClientProfile(string firstName, string lastName)
    {
        if (UserType is not null)
            return Result.Failure("Profile already exists");

        ClientProfile = ClientProfile.Create(Id, firstName, lastName);
        UserType = Entities.UserType.Client;
        return Result.Success();
    }

    public Result AddBusinessProfile(string companyName)
    {
        if (UserType is not null)
            return Result.Failure("Profile already exists");

        BusinessProfile = BusinessProfile.Create(Id, companyName);
        UserType = Entities.UserType.Business;
        return Result.Success();
    }
}
