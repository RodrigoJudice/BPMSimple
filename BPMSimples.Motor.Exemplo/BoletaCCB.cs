using BPMSimples.Motor.Boleta.Config;
using BPMSimples.Motor.Boleta.Repositorio;
using BPMSimples.Motor.Dominio;
using BPMSimples.Motor.Interfaces;
using BPMSimples.Motor.MaquinaEstado;

namespace BPMSimples.Motor.Boleta;

public class BoletaCCB : StateMachineBPM
{
    public BoletaBancaria DadosBoleta { get; set; } = default!;

    public BoletaCCB(long idInstancia, EstadoBPM estadoInicial, ISegurancaBPM seguranca, IMotorAlcada? alcada = null, PersistenciaStateMachine? persistencia = null)
        : base(idInstancia, estadoInicial, seguranca, alcada)
    {
        _callbackPersistencia = new PersistenciaStateMachine(async instancia =>
        {
            var repo = new BoletaCCBRepositoryFake();
            await repo.SalvarAsync((BoletaCCB)instancia);
        });

    }

    protected override decimal ObterValorOperacao()
    {
        return 200_000;
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
            WorkflowBoletaConfig.Estados.Estado2, null, false, ["enviar"]);

        RegistrarTransicao(
            WorkflowBoletaConfig.Estados.Estado2,
            WorkflowBoletaConfig.Eventos.Aprovar,
            WorkflowBoletaConfig.Estados.Estado3, null, true, ["aprovar"]);

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
