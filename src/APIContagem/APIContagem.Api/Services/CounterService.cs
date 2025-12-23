namespace APIContagem.Api.Services;

public class CounterService : ServiceBase
{
    private int _valorAtual = 20000;

    public int ValorAtual { get => _valorAtual; }

    public void Incrementar()
    {
        _valorAtual++;
    }
}