namespace BPMSimples.Motor.Alcadas;

// Resultado da verificação de alçada
public class StatusAlcada
{
    public bool AlcadaCompleta => Faltantes.Count == 0;
    public List<string> JaExecutadas { get; set; } = new();
    public List<string> Faltantes { get; set; } = new();
}
