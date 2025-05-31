using CommertialPortal_WebAPI.Features.Analitics;
using CommertialPortal_WebAPI.Features.Analitics.GetAnalitics;
using CommertialPortal_WebAPI.Features.Analitics.IncrementPromo;
using CommertialPortal_WebAPI.Features.Analitics.IncrementView;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommertialPortal_WebAPI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnaliticsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnaliticsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("{postId}/view")]
    public async Task<IActionResult> RegisterView(int postId)
    {
        var result = await _mediator.Send(new IncrementViewCountCommand(postId));
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpPost("{postId}/promo-copy")]
    public async Task<IActionResult> RegisterPromoCopy(int postId)
    {
        var result = await _mediator.Send(new IncrementPromoCopiedCommand(postId));
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    /// <summary>
    /// Получение аналитики для бизнес-профиля текущего пользователя
    /// </summary>
    [HttpGet]
    [Authorize(Roles = AuthRoles.Business)]
    public async Task<IActionResult> GetAnalytics()
    {
        var result = await _mediator.Send(new GetBusinessAnalyticsQuery());
        return Ok(result);
    }

}
