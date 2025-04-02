namespace BPMSimples.Motor.Boleta.Repositorio;

public class BoletaCCBRepositoryFake : IBoletaCCBRepository
{
    public List<BoletaCCB> BoletasSalvas { get; } = [];

    public Task SalvarAsync(BoletaCCB boleta)
    {
        return Task.CompletedTask;
    }

}
