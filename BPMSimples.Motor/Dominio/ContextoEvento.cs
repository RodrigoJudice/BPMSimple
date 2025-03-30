namespace BPMSimples.Motor.Dominio;

public class ContextoEvento
{
    public string Usuario { get; }
    public string Justificativa { get; }
    public ContextoEvento(string usuario, string justificativa)
    {
        Usuario = usuario;
        Justificativa = justificativa;
    }
}
