using CommertialPortal_WebAPI.Features.Users.GetBusinesses;
using MediatR;

namespace CommertialPortal_WebAPI.Features.Users.GetBusinessById
{
    public class GetBusinessByIdQuery : IRequest<BusinessProfileDto?>
    {
        public int Id { get; }

        public GetBusinessByIdQuery(int id)
        {
            Id = id;
        }
    }
}
