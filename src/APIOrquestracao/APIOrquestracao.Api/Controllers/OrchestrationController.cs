using APIOrquestracao.Api.Clients;
using APIOrquestracao.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIOrquestracao.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrchestrationController : ControllerBase
{
    private readonly ILogger<OrchestrationController> _logger;
    private readonly IConfiguration _configuration;
    private readonly ContagemClient _contagemClient;

    public OrchestrationController(ILogger<OrchestrationController> logger,
        IConfiguration configuration, ContagemClient contagemClient)
    {
        _logger = logger;
        _configuration = configuration;
        _contagemClient = contagemClient;
    }

    [HttpGet]
    public async Task<ResultadoOrquestracao> Get()
    {
        var resultado = new ResultadoOrquestracao
        {
            Horario = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        var urlApiContagem = _configuration["ApiContagem"]!;
        resultado.ContagemPostgres =
            await _contagemClient.ObterContagemAsync(urlApiContagem + "/Counter");
        _logger.LogInformation($"Valor contagem Redis: {resultado.ContagemPostgres!.ValorAtual}");
        
        return resultado;
    }
}
