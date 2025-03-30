using BPMSimples.Motor.Dominio;

namespace BPMSimples.Motor.Interfaces;

public interface ISegurancaBPM
{
    bool PodeExecutarEvento(EstadoBPM estadoAtual, EventoBPM evento, string usuario);
}
