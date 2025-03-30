namespace BPMSimples.Motor.Alcadas;

// Requisito de alçada para determinado valor
public class RequisitoAlcada
{
    public decimal ValorMin { get; set; }
    public decimal ValorMax { get; set; }
    // Cada grupo é uma lista de usuários que precisam aprovar
    public List<List<string>> GruposDeUsuarios { get; set; } = [];
}