using BPMSimples.Motor.Boleta.Config;

namespace BPMSimples.Motor.Boleta.Teste;

internal class Program
{
    static async Task Main(string[] args)
    {
        var alcada = new MotorAlcadaTransacional();
        var seguranca = new SegurancaBoleta(
            userName: "joao",
            email: "joao@banco.com",
            grupos: new[] { "Mesa", "Risco" },
            transacoes: new[] { "aprova_100_000", "aprova_500_000" }
        );


        BoletaCCB boleta = new(1, WorkflowBoletaConfig.Estados.Estado1, seguranca, alcada);

        await boleta.ExecutarEventoAsync(WorkflowBoletaConfig.Eventos.Salvar, "justificativa 1", 1);
        await boleta.ExecutarEventoAsync(WorkflowBoletaConfig.Eventos.Enviar, "justificativa 2", 2);
        await boleta.ExecutarEventoAsync(WorkflowBoletaConfig.Eventos.Aprovar, "justificativa 3", 3);


        Console.WriteLine(boleta.ObterEstadoAtual());
    }
}
