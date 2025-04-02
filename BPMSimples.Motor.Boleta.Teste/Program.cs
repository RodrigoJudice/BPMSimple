using BPMSimples.Motor.Boleta.Config;

namespace BPMSimples.Motor.Boleta.Teste;

internal class Program
{
    static async Task Main()
    {
        var alcada = new MotorAlcadaTransacional();
        var seguranca = new SegurancaBoleta(
            userName: "vp1",
            email: "vp1@banco.com",
            grupos: ["Mesa", "Risco"],
            transacoes: ["enviar", "aprovar"]
        );

        Console.WriteLine(alcada.ObterGruposFormatadosParaDebug(200_000));
        Console.WriteLine(alcada.FormatarGruposComoTexto(200_000));


        BoletaCCB boleta = new(1, WorkflowBoletaConfig.Estados.Estado1, seguranca, alcada);

        await boleta.ExecutarEventoAsync(WorkflowBoletaConfig.Eventos.Salvar, "justificativa 1", 1);
        await boleta.ExecutarEventoAsync(WorkflowBoletaConfig.Eventos.Enviar, "justificativa 2", 2);
        await boleta.ExecutarEventoAsync(WorkflowBoletaConfig.Eventos.Aprovar, "justificativa 3", 3);


        Console.WriteLine(boleta.ObterEstadoAtual());
    }
}
