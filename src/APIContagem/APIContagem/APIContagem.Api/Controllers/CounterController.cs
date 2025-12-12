using APIContagem.Api.Configurations.Loggings;
using APIContagem.Api.Data;
using APIContagem.Api.Models;
using APIContagem.Api.Services;
using Microsoft.AspNetCore.Mvc;


namespace APIContagem.Controllers;

[ApiController]
[Route("[controller]")]
public class CounterController : ControllerBase
{
    private static readonly CounterService _CONTADOR = new();
    private readonly ILogger<CounterController> _logger;
    private readonly CounterRepository _repository;

    public CounterController(ILogger<CounterController> logger,
        CounterRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    public CountResult Get()
    {
        int valorAtualContador;

        lock (_CONTADOR)
        {
            _CONTADOR.Incrementar();
            valorAtualContador = _CONTADOR.ValorAtual;
        }

        _logger.LogValorAtual(valorAtualContador);

        var resultado = new CountResult()
        {
            ValorAtual = valorAtualContador,
            Producer = _CONTADOR.Local,
            Kernel = _CONTADOR.Kernel,
            Framework = _CONTADOR.Framework,
            Mensagem = "Testes com .NET 8 + ASP.NET Core + PostgreSQL"
        };
        _repository.Insert(resultado);
        _logger.LogInformation($"Registro inserido com sucesso! Valor: {valorAtualContador}");

        return resultado;
    }
}