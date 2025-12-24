using Microsoft.AspNetCore.Mvc;


namespace APIContagem.Controllers;

public enum LogType
{
    Error,
    Warning,
    Information,
    Debug
}

public class LogRequest
{
    public LogType LogType { get; set; }
    public string? Message { get; set; }
}

[ApiController]
[Route("[controller]")]
public class LoggerController : ControllerBase
{
    private readonly ILogger<LoggerController> _logger;

    public LoggerController(ILogger<LoggerController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Post([FromBody] LogRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("Request must contain a LogType and a non-empty Message.");

        switch (request.LogType)
        {
            case LogType.Error:
                _logger.LogError(request.Message);
                break;
            case LogType.Warning:
                _logger.LogWarning(request.Message);
                break;
            case LogType.Information:
                _logger.LogInformation(request.Message);
                break;
            case LogType.Debug:
                _logger.LogDebug(request.Message);
                break;
            default:
                _logger.LogInformation(request.Message);
                break;
        }

        return Ok(new { status = "Logged", level = request.LogType.ToString(), message = request.Message });
    }
}