using MediatR;

namespace CommertialPortal_WebAPI.Features.Analitics.GetAnalitics;

public record GetBusinessAnalyticsQuery() : IRequest<BusinessAnaliticsDto>
{
}
