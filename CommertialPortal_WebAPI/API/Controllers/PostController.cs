using System.Security.Claims;
using CommertialPortal_WebAPI.Domain.Entities;
using CommertialPortal_WebAPI.Features.Posts.AddFavouritePost;
using CommertialPortal_WebAPI.Features.Posts.CreateBranch;
using CommertialPortal_WebAPI.Features.Posts.CreatePost;
using CommertialPortal_WebAPI.Features.Posts.GetBranches;
using CommertialPortal_WebAPI.Features.Posts.GetBranchesByBusinessId;
using CommertialPortal_WebAPI.Features.Posts.GetFavouritePosts;
using CommertialPortal_WebAPI.Features.Posts.GetPostById;
using CommertialPortal_WebAPI.Features.Posts.GetPosts;
using CommertialPortal_WebAPI.Features.Posts.RemoveFavouritePost;
using CommertialPortal_WebAPI.Features.Users.GetBusinessById;
using CommertialPortal_WebAPI.Features.Users.GetBusinesses;
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
    /// Создание нового поста (акция, событие, скидка).
    /// </summary>
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
    /// Создание нового поста (акция, событие, скидка).
    /// </summary>
    /// <returns>Идентификатор созданного поста.</returns>
    [HttpGet("favourites")]
    [Authorize(Roles = AuthRoles.Client)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<List<int>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetFavourites()
    {
        var posts = await _mediator.Send(new GetFavouritePostsQuerry());
        return Ok(posts);
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

    /// <summary>
    /// Получение всех постов.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<PostDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPosts()
    {
        var posts = await _mediator.Send(new GetPostsQuery());
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetPostById(int id)
    {
        var post = await _mediator.Send(new GetPostByIdQuery(id));

        if (post == null)
        {
            return NotFound();
        }

        return Ok(post);
    }

}
