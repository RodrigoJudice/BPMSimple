using BPMSimples.Motor.Interfaces;

namespace BPMSimples.Motor.Boleta;

public class SegurancaBoleta : ISegurancaBPM
{
    public string UserName { get; }
    public string Email { get; }
    public List<string> Grupos { get; }
    public List<string> Transacoes { get; }

    public SegurancaBoleta(string userName, string email, IEnumerable<string>? grupos = null, IEnumerable<string>? transacoes = null)
    {
        UserName = userName;
        Email = email;
        Grupos = grupos?.ToList() ?? new();
        Transacoes = transacoes?.ToList() ?? new();
    }

    public bool EstaNoGupo(string grupo) =>
        Grupos.Contains(grupo, StringComparer.OrdinalIgnoreCase);

    public bool TemTransacao(string transacao) =>
        Transacoes.Contains(transacao, StringComparer.OrdinalIgnoreCase);

}