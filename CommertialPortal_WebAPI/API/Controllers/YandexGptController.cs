namespace CommertialPortal_WebAPI.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using CommertialPortal_WebAPI.Features.YandexGpt;

[ApiController]
[Route("api/[controller]")]
public class YandexGptController : ControllerBase
{
    private readonly IYandexGptService _gpt;

    public YandexGptController(IYandexGptService gpt)
    {
        _gpt = gpt;
    }

    /// <summary>
    /// Отправляет запрос к YandexGPT и возвращает ответ ИИ.
    /// </summary>
    /// <param name="request">Промпт от пользователя.</param>
    /// <returns>Ответ от модели.</returns>
    [HttpPost("ask")]
    [ProducesResponseType(typeof(YandexGptResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Ask([FromBody] YandexGptRequest request)
    {
        var reply = await _gpt.AskAsync(request.Prompt);
        return Ok(new YandexGptResponse(reply));
    }
}

