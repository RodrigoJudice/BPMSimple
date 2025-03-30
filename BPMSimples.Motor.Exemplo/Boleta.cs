using BPMSimples.Motor.Dominio;
using BPMSimples.Motor.Interfaces;
using BPMSimples.Motor.MaquinaEstado;

namespace BPMSimples.Motor.Boleta;

public class Boleta(long idInstancia, EstadoBPM estadoInicial, ISegurancaBPM seguranca, IMotorAlcada? alcada = null) : StateMachineBPM(idInstancia, estadoInicial, seguranca, alcada)
{
    protected override void ConfigurarWorkflow()
    {
        throw new NotImplementedException();
    }

    protected override decimal ObterValorOperacao()
    {
        throw new NotImplementedException();
    }
}
