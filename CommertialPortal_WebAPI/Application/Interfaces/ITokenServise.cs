using CommertialPortal_WebAPI.Domain.Entities;

namespace CommertialPortal_WebAPI.Application.Interfaces;

public interface ITokenServise
{
    string CreateToken(User user);
}
