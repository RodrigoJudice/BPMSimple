namespace BPMSimples.Motor.MaquinaEstado;

using BPMSimples.Motor.Alcadas;
using BPMSimples.Motor.Dominio;
using BPMSimples.Motor.Interfaces;
using Stateless;

public abstract class StateMachineBPM
{
    public long IdInstancia { get; }
    public int Versao { get; private set; } = 1;

    protected EstadoBPM EstadoAtual { get; set; }
    protected readonly StateMachine<EstadoBPM, EventoBPM> _maquina;
    private readonly ISegurancaBPM _seguranca;
    private readonly IMotorAlcada? _motorAlcada;

    public List<TransicaoLogBPM> Transicoes { get; } = [];
    private readonly List<RegistroAlcada> _aprovacoes = [];

    private readonly Dictionary<TransicaoBPM, Func<Task<EventoBPM?>>> _acoesPorTransicao = [];

    public static readonly EventoBPM Evento_NULL = new(null);

    protected StateMachineBPM(long idInstancia, EstadoBPM estadoInicial, ISegurancaBPM seguranca, IMotorAlcada? motorAlcada = null)
    {
        IdInstancia = idInstancia;
        EstadoAtual = estadoInicial;
        _seguranca = seguranca;
        _motorAlcada = motorAlcada;

        _maquina = new StateMachine<EstadoBPM, EventoBPM>(() => EstadoAtual, s => EstadoAtual = s);

        ConfigurarWorkflow();
        ConfigurarMaquina();
    }

    protected abstract void ConfigurarWorkflow();
    protected abstract decimal ObterValorOperacao();

    private void ConfigurarMaquina()
    {
        var agrupadoPorEstadoOrigem = _acoesPorTransicao
            .GroupBy(x => x.Key.Origem);

        foreach (var grupo in agrupadoPorEstadoOrigem)
        {
            var estadoOrigem = grupo.Key;
            var configuracao = _maquina.Configure(estadoOrigem);

            foreach (var (key, _) in grupo)
            {
                if (key.Origem == key.Destino)
                    configuracao.PermitReentry(key.Evento);
                else
                    configuracao.Permit(key.Evento, key.Destino);
            }
        }
    }

    protected void RegistrarTransicao(
    EstadoBPM origem,
    EventoBPM evento,
    EstadoBPM destino,
    Func<Task<EventoBPM?>>? acao = null,
    bool requerAlcada = false,
    List<string>? transacoesPermitidas = null)
    {
        var definicao = new TransicaoBPM(origem, evento, destino, requerAlcada)
        {
            TransacoesPermitidas = transacoesPermitidas ?? []
        };
        _acoesPorTransicao[definicao] = acao ?? (() => Task.FromResult<EventoBPM?>(Evento_NULL));
    }

    public async Task<EventoBPM?> ExecutarEventoAsync(EventoBPM evento, string justificativa, int versaoEsperada)
    {
        if (!VersaoEhValida(versaoEsperada))
            throw new InvalidOperationException("Versão desatualizada.");

        if (!_maquina.CanFire(evento))
            throw new InvalidOperationException($"Evento '{evento.Nome}' não pode ser executado no estado '{EstadoAtual.Nome}'.");

        var estadoAntes = EstadoAtual;
        var transicaoEncontrada = _acoesPorTransicao.Keys.FirstOrDefault(k => k.Origem == estadoAntes && k.Evento == evento);

        if (transicaoEncontrada is not null && transicaoEncontrada.TransacoesPermitidas.Count != 0)
        {
            var possuiPermissao = transicaoEncontrada.TransacoesPermitidas.Any(_seguranca.TemTransacao);
            if (!possuiPermissao)
                throw new UnauthorizedAccessException($"Usuário '{_seguranca.UserName}' não tem nenhuma das transações permitidas para executar '{evento.Nome}' no estado '{EstadoAtual.Nome}'.");
        }

        if (transicaoEncontrada is not null && transicaoEncontrada.RequerAlcada)
        {
            if (_motorAlcada == null)
                throw new InvalidOperationException("Motor de alçada não configurado.");

            var status = _motorAlcada.ObterStatus(ObterValorOperacao(), _aprovacoes, _seguranca);

            foreach (var usuario in status.JaExecutadas)
            {
                if (!_aprovacoes.Any(a => a.Usuario == usuario))
                    _aprovacoes.Add(new RegistroAlcada(_seguranca.UserName));
            }

            if (!status.AlcadaCompleta)
            {
                Console.WriteLine("🔒 Alçada incompleta:");
                Console.WriteLine("✅ Já executadas: " + string.Join(", ", status.JaExecutadas));
                Console.WriteLine("❌ Faltantes: " + string.Join(", ", status.Faltantes));
                return null;
            }

        }

        await _maquina.FireAsync(evento);
        Versao++;

        Transicoes.Add(new TransicaoLogBPM
        {
            De = estadoAntes,
            Para = EstadoAtual,
            Evento = evento,
            Usuario = _seguranca.UserName,
            Justificativa = justificativa,
            Data = DateTime.UtcNow
        });

        if (transicaoEncontrada != null && _acoesPorTransicao.TryGetValue(transicaoEncontrada, out var acao))
        {
            var proximo = await acao();
            return proximo == Evento_NULL ? null : proximo;
        }

        return null;
    }


    public IEnumerable<EventoBPM> ObterEventosPermitidos(EstadoBPM estado)
    {
        return _acoesPorTransicao
            .Where(x => x.Key.Origem == estado)
            .Select(x => x.Key.Evento)
            .Distinct();
    }

    public IEnumerable<EventoBPM> ObterEventosPermitidosAtuais() => _maquina.PermittedTriggers;

    public string ObterEstadoAtual() => EstadoAtual.Nome;

    private bool VersaoEhValida(int versaoEsperada) => Versao == versaoEsperada;
}
