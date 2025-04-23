using CommertialPortal_WebAPI.Domain.Entities;

namespace CommertialPortal_WebAPI.Application.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}
