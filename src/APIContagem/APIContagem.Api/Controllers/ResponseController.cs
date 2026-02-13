using APIContagem.Api.Models;
using Microsoft.AspNetCore.Mvc;


namespace APIContagem.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ResponseController : ControllerBase
{
    private readonly ILogger<ResponseController> _logger;

    public ResponseController(ILogger<ResponseController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Post([FromBody] ResponseRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("Request must contain a LogType and a non-empty Message.");

        switch (request.ResponseType)
        {
            case ResponseType.ServerError:
                _logger.LogError(request.Message);
                return StatusCode(500, new { status = "Error", level = "Error", message = request.Message });
            case ResponseType.BadRequest:
                _logger.LogWarning(request.Message);
                return StatusCode(400, new { status = "BadRequest", level = "Warning", message = request.Message });
            case ResponseType.NotFound:
                _logger.LogInformation(request.Message);
                return StatusCode(404, new { status = "NotFound", level = "Information", message = request.Message });
            case ResponseType.Ok:
                _logger.LogDebug(request.Message);
                return StatusCode(200, new { status = "Ok", level = "Debug", message = request.Message });
            default:
                return StatusCode(200, new { status = "Ok", level = "Debug", message = "Erro fora da lista" });
        }
    }
}