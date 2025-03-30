namespace BPMSimples.Motor.MaquinaEstado;

using BPMSimples.Motor.Dominio;
using BPMSimples.Motor.Interfaces;
using Stateless;

public abstract class StateMachineBPM
{
    public long IdInstancia { get; }
    public int Versao { get; private set; } = 1;

    protected EstadoBPM EstadoAtual { get; set; }

    protected readonly StateMachine<EstadoBPM, EventoBPM> _maquina;

    public List<TransicaoBPM> Transicoes { get; } = [];

    private readonly ISegurancaBPM? _seguranca;

    private readonly Dictionary<(EstadoBPM, EventoBPM, EstadoBPM), Func<Task<EventoBPM?>>> _acoesPorTransicao = new();

    public static readonly EventoBPM Evento_NULL = new("NULL");

    protected StateMachineBPM(long idInstancia, EstadoBPM estadoInicial, ISegurancaBPM? seguranca = null)
    {
        IdInstancia = idInstancia;
        EstadoAtual = estadoInicial;
        _seguranca = seguranca;

        _maquina = new StateMachine<EstadoBPM, EventoBPM>(() => EstadoAtual, s => EstadoAtual = s);

        ConfigurarWorkflow();
        ConfigurarMaquina();
    }

    protected abstract void ConfigurarWorkflow();

    private void ConfigurarMaquina()
    {
        var agrupadoPorEstadoOrigem = _acoesPorTransicao
            .GroupBy(x => x.Key.Item1); // EstadoInicial

        foreach (var grupo in agrupadoPorEstadoOrigem)
        {
            var estadoOrigem = grupo.Key;
            var configuracao = _maquina.Configure(estadoOrigem);

            foreach (var ((_, evento, destino), _) in grupo)
            {
                if (estadoOrigem == destino)
                {
                    configuracao.PermitReentry(evento);
                }
                else
                {
                    configuracao.Permit(evento, destino);
                }
            }
        }
    }

    protected void RegistrarTransicao(
        EstadoBPM origem,
        EventoBPM evento,
        EstadoBPM destino,
        Func<Task<EventoBPM?>>? acao = null)
    {
        _acoesPorTransicao[(origem, evento, destino)] = acao ?? (() => Task.FromResult<EventoBPM?>(Evento_NULL));
    }

    public async Task<EventoBPM?> ExecutarEventoAsync(EventoBPM evento, string usuario, string justificativa, int versaoEsperada)
    {
        if (!VersaoEhValida(versaoEsperada))
            throw new InvalidOperationException("Versão desatualizada.");

        if (_seguranca != null && !_seguranca.PodeExecutarEvento(EstadoAtual, evento, usuario))
            throw new UnauthorizedAccessException($"Usuário '{usuario}' não tem permissão para executar '{evento.Nome}' no estado '{EstadoAtual.Nome}'.");

        if (!_maquina.CanFire(evento))
            throw new InvalidOperationException($"Evento '{evento.Nome}' não pode ser executado no estado '{EstadoAtual.Nome}'.");

        var estadoAntes = EstadoAtual;

        await _maquina.FireAsync(evento);
        Versao++;

        Transicoes.Add(new TransicaoBPM
        {
            De = estadoAntes,
            Para = EstadoAtual,
            Evento = evento,
            Usuario = usuario,
            Justificativa = justificativa,
            Data = DateTime.UtcNow
        });

        var chave = (estadoAntes, evento, EstadoAtual);
        if (_acoesPorTransicao.TryGetValue(chave, out var acao))
        {
            var proximo = await acao();
            return proximo == Evento_NULL ? null : proximo;
        }

        return null;
    }

    public string ObterEstadoAtual() => EstadoAtual.Nome;


    private bool VersaoEhValida(int versaoEsperada) => Versao == versaoEsperada;
}

