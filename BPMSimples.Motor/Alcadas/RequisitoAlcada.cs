namespace BPMSimples.Motor.Alcadas;

// Requisito de alçada para determinado valor
public class RequisitoAlcada
{
    public decimal ValorMin { get; set; }
    public decimal ValorMax { get; set; }
    public List<string> Usuarios { get; set; } = new();
}