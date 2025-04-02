namespace BPMSimples.Motor.Boleta.Repositorio;
public interface IBoletaCCBRepository
{
    Task SalvarAsync(BoletaCCB boleta);
}
