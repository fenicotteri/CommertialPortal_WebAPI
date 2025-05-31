using CommertialPortal_WebAPI.Features.Posts.CreateBranch;
using System.Security.Claims;
using CommertialPortal_WebAPI.Features.Posts.GetBranches;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommertialPortal_WebAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BranchController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Создание нового филиала для бизнеса.
        /// </summary>
        /// <param name="request">Данные филиала.</param>
        /// <returns>Идентификатор созданного филиала.</returns>
        [HttpPost]
        [Authorize(Roles = AuthRoles.Business)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateBranch([FromBody] CreateBrunchRequest request)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email is null)
                return Unauthorized(ApiResponse<string>.FailureResponse("User email not found."));

            var result = await _mediator.Send(new CreateBranchCommand(email, request));
            if (result.IsFailure)
                return BadRequest(ApiResponse<string>.FailureResponse(result.Error));

            return Ok(ApiResponse<int>.SuccessResponse(result.Value));
        }

        [HttpGet]
        public async Task<ActionResult<List<BranchDto>>> GetBranches()
        {
            var branches = await _mediator.Send(new GetBranchesQuery());
            return Ok(branches);
        }
    }
}
