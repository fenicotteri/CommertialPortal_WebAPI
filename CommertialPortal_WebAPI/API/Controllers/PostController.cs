using System.Security.Claims;
using CommertialPortal_WebAPI.Features.Posts.CreateBranch;
using CommertialPortal_WebAPI.Features.Users.RegisterBusiness;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommertialPortal_WebAPI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostController(IMediator mediator) => _mediator = mediator;

    [HttpPost("branch")]
    [Authorize(Roles = AuthRoles.Business)]
    public async Task<IActionResult> CreateBranch([FromBody] CreateBrunchRequest request)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        var result = await _mediator.Send(new CreateBranchCommand(email, request));
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
