using Microsoft.AspNetCore.Mvc;


namespace APIContagem.Controllers;

[ApiController]
[Route("[controller]")]
public class LoggerController : ControllerBase
{
    private readonly ILogger<LoggerController> _logger;

    public LoggerController(ILogger<LoggerController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public string Get()
    {
        
        _logger.LogWarning("Warning Registrado com sucesso!");

        return "Warning Registrado com sucesso!";
    }
}