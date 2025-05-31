using System.Security.Claims;
using CommertialPortal_WebAPI.Features.Auth.GetMe;
using CommertialPortal_WebAPI.Features.Users.GetMeUser;
using CommertialPortal_WebAPI.Features.Users.LoginUser;
using CommertialPortal_WebAPI.Features.Users.RegisterBusiness;
using CommertialPortal_WebAPI.Features.Users.RegisterClient;
using CommertialPortal_WebAPI.Features.Users.SubscribeToBusiness;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommertialPortal_WebAPI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Регистрация нового бизнес-пользователя.
    /// </summary>
    /// <param name="command">Данные для регистрации бизнеса.</param>
    /// <returns>Токен доступа.</returns>
    /// <response code="200">Успешная регистрация бизнес-пользователя.</response>
    /// <response code="400">Ошибка в данных запроса или регистрация невозможна.</response>
    [HttpPost("business/register")]
    [ProducesResponseType(typeof(RegisterBusinessResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterBusiness([FromBody] RegisterBusinessCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    /// <summary>
    /// Регистрация нового клиента.
    /// </summary>
    /// <param name="command">Данные для регистрации клиента.</param>
    /// <returns>Токен доступа.</returns>
    /// <response code="200">Успешная регистрация клиента.</response>
    /// <response code="400">Ошибка в данных запроса или регистрация невозможна.</response>
    [HttpPost("client/register")]
    [ProducesResponseType(typeof(RegisterClientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterClient([FromBody] RegisterClientCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    /// <summary>
    /// Проверка валидности токена пользователя.
    /// </summary>
    /// <returns>Информация о пользователе, если токен действителен.</returns>
    /// <response code="200">Токен действителен.</response>
    /// <response code="401">Неавторизованный доступ.</response>
    [Authorize]
    [HttpGet("check-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult CheckToken()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

        return Ok(new
        {
            Message = "Token is valid",
            Email = email,
            Roles = roles
        });
    }

    /// <summary>
    /// Авторизация пользователя.
    /// </summary>
    /// <param name="command">Данные для входа пользователя.</param>
    /// <returns>Токен доступа.</returns>
    /// <response code="200">Успешная авторизация.</response>
    /// <response code="400">Неверный логин или пароль.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    /// <summary>
    /// Подписка клиента на выбранный бизнес.
    /// </summary>
    /// <param name="command">Данные о бизнесе для подписки (идентификатор бизнес-профиля).</param>
    /// <returns>Результат операции подписки.</returns>
    /// <response code="200">Подписка успешно оформлена.</response>
    /// <response code="400">Ошибка запроса: бизнес-профиль не найден или клиент уже подписан.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    [HttpPost("subscribe")]
    [Authorize(Roles = AuthRoles.Client)]
    public async Task<IActionResult> SubscribeToBusiness([FromBody] SubscribeToBusinessCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(ApiResponse<string>.FailureResponse(result.Error));

        return Ok(ApiResponse<string>.SuccessResponse("Successfully subscribed."));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<GetMeDto>> GetMe()
    {
        var user = await _mediator.Send(new GetMeQuery());
        return Ok(user);
    }

}
