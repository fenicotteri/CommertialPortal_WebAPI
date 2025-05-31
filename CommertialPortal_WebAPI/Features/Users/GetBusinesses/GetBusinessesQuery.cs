using MediatR;
using System.Collections.Generic;

namespace CommertialPortal_WebAPI.Features.Users.GetBusinesses
{
    public class GetBusinessesQuery : IRequest<List<BusinessProfileDto>>
    {
    }
}
