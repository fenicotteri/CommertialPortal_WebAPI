using System.Security.Claims;
using CommertialPortal_WebAPI.Features.Posts.AddFavouritePost;
using CommertialPortal_WebAPI.Features.Posts.CreateBranch;
using CommertialPortal_WebAPI.Features.Posts.CreatePost;
using CommertialPortal_WebAPI.Features.Posts.RemoveFavouritePost;
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

    /// <summary>
    /// Создание нового филиала для бизнеса.
    /// </summary>
    /// <param name="request">Данные филиала.</param>
    /// <returns>Идентификатор созданного филиала.</returns>
    [HttpPost("branch")]
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

    /// <summary>
    /// Создание нового поста (акция, событие, скидка).
    /// </summary>
    /// <param name="command">Данные поста.</param>
    /// <returns>Идентификатор созданного поста.</returns>
    [HttpPost("create")]
    [Authorize(Roles = AuthRoles.Business)]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<int>.SuccessResponse(result));
    }

    /// <summary>
    /// Добавить пост в избранные.
    /// </summary>
    /// <param name="postId">ID поста.</param>
    /// <returns>204 No Content при успехе.</returns>
    [HttpPost("favourites/{postId:int}")]
    [Authorize(Roles = AuthRoles.Client)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddPostToFavourites(int postId)
    {
        await _mediator.Send(new AddFavouritePostCommand(postId));
        return NoContent();
    }

    /// <summary>
    /// Удалить пост из избранных.
    /// </summary>
    /// <param name="postId">ID поста.</param>
    /// <returns>204 No Content при успехе.</returns>
    [HttpDelete("favourites/{postId:int}")]
    [Authorize(Roles = AuthRoles.Client)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RemovePostFromFavourites(int postId)
    {
        await _mediator.Send(new RemoveFavouritePostCommand(postId));
        return NoContent();
    }
}
