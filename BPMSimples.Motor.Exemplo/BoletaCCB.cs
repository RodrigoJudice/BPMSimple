using BPMSimples.Motor.Boleta.Config;
using BPMSimples.Motor.Dominio;
using BPMSimples.Motor.Interfaces;
using BPMSimples.Motor.MaquinaEstado;

namespace BPMSimples.Motor.Boleta;

public class BoletaCCB : StateMachineBPM
{
    public BoletaCCB(long idInstancia, EstadoBPM estadoInicial, ISegurancaBPM? seguranca = null) : base(idInstancia, estadoInicial, seguranca)
    {
    }
    protected override void ConfigurarWorkflow()
    {

        RegistrarTransicao(
            WorkflowBoletaConfig.Estados.Estado1,
            WorkflowBoletaConfig.Eventos.Salvar,
            WorkflowBoletaConfig.Estados.Estado1);

        RegistrarTransicao(
            WorkflowBoletaConfig.Estados.Estado1,
            WorkflowBoletaConfig.Eventos.Enviar,
            WorkflowBoletaConfig.Estados.Estado2);

        RegistrarTransicao(
            WorkflowBoletaConfig.Estados.Estado2,
            WorkflowBoletaConfig.Eventos.Aprovar,
            WorkflowBoletaConfig.Estados.Estado3);

        RegistrarTransicao(
            WorkflowBoletaConfig.Estados.Estado3,
            WorkflowBoletaConfig.Eventos.Reprovar,
            WorkflowBoletaConfig.Estados.Estado1);

        RegistrarTransicao(
            WorkflowBoletaConfig.Estados.Estado3,
            WorkflowBoletaConfig.Eventos.Aprovar,
            WorkflowBoletaConfig.Estados.Estado4,
            async () =>
            {
                Console.WriteLine("Encadeando evento Finalizar...");
                return WorkflowBoletaConfig.Eventos.Finalizar;
            });

        RegistrarTransicao(
            WorkflowBoletaConfig.Estados.Estado4,
            WorkflowBoletaConfig.Eventos.Finalizar,
            WorkflowBoletaConfig.Estados.Estado5);
    }
}
