namespace BPMSimples.Motor.Alcadas;

public class RegistroAlcada(string usuario)
{
    public string Usuario { get; set; } = usuario;
    public DateTime Data { get; set; } = DateTime.UtcNow;
}
