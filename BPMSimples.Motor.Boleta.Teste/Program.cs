using BPMSimples.Motor.Boleta.Config;

namespace BPMSimples.Motor.Boleta.Teste;

internal class Program
{
    static async Task Main(string[] args)
    {
        BoletaCCB boleta = new BoletaCCB(1, WorkflowBoletaConfig.Estados.Estado1);

        await boleta.ExecutarEventoAsync(WorkflowBoletaConfig.Eventos.Salvar, "usuario", "justificativa", 1);

        Console.WriteLine(boleta.ObterEstadoAtual());
    }
}
