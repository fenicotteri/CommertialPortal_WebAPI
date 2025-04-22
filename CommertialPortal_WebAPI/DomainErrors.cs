using CommertialPortal_WebAPI.Application.Common;

namespace CommertialPortal_WebAPI;

public class DomainErrors
{
    public static class User
    {
        public static readonly Func<string, Error> NotFound = id => new Error(
            "User.NotFound",
            $"The user with the identifier {id} was not found.");

        public static readonly Error EmailAlreadyInUse = new(
            "User.EmailAlreadyInUse",
            "The specified email is already in use.");

        public static readonly Error FailedRegistration = new(
            "User.FailedRegistration",
            "Something went wrong while adding the user.");

        public static readonly Func<string, Error> EmailNotFound = email => new Error(
            "User.BusinessProfile.NotFound",
            "Business profile not found for the given email.");

    }
}
