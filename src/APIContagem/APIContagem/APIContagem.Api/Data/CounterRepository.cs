using APIContagem.Api.Models;

namespace APIContagem.Api.Data;

public class CounterRepository
{
    private readonly DatabaseContext _context;

    public CounterRepository(DatabaseContext context)
    {
        _context = context;
    }

    public void Insert(CountResult resultado)
    {
        _context.Historicos!.Add(new()
        {
            DataProcessamento = DateTime.Now,
            ValorAtual = resultado.ValorAtual,
            Producer = resultado.Producer,
            Kernel = resultado.Kernel,
            Framework = resultado.Framework,
            Mensagem = resultado.Mensagem
        });
        _context.SaveChanges();
    }
}