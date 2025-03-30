namespace BPMSimples.Motor.Dominio;

public class TransicaoLogBPM
{
    public EstadoBPM De { get; set; }
    public EstadoBPM Para { get; set; }
    public EventoBPM Evento { get; set; }
    public string? Justificativa { get; set; }
    public string? Usuario { get; set; }
    public DateTime Data { get; set; }
}
