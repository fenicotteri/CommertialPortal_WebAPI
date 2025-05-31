using CommertialPortal_WebAPI.Features.Users.GetMeUser;
using CommertialPortal_WebAPI.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CommertialPortal_WebAPI.Features.Auth.GetMe;

public class GetMeQueryHandler : IRequestHandler<GetMeQuery, GetMeDto>
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetMeQueryHandler(DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetMeDto> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var email = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

        var user = await _context.Users
            .Include(u => u.BusinessProfile)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (user == null)
            throw new Exception("Пользователь не найден");

        return new GetMeDto
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            UserType = user.UserType.ToString(), // Enum в строку ("Client" / "Business")
            ProfileId = user.BusinessProfile?.Id ?? user.ClientProfile?.Id,
        };
    }
}
