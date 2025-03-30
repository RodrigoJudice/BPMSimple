namespace BPMSimples.Motor.Dominio;

public record TransicaoBPM(EstadoBPM Origem, EventoBPM Evento, EstadoBPM Destino, bool RequerAlcada)
{
    public List<string> TransacoesPermitidas { get; init; } = [];
}