namespace BPMSimples.Motor.Interfaces;

public interface ISegurancaBPM
{
    string UserName { get; }
    string Email { get; }
    List<string> Grupos { get; }
    List<string> Transacoes { get; }
    bool EstaNoGupo(string grupo);
    bool TemTransacao(string transacao);
}