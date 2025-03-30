using BPMSimples.Motor.Alcadas;

namespace BPMSimples.Motor.Interfaces;


// Interface do motor de alçada
public interface IMotorAlcada
{
    public StatusAlcada ObterStatus(decimal valor, ISegurancaBPM seguranca);
    RequisitoAlcada? ObterRegra(decimal valor);
}
