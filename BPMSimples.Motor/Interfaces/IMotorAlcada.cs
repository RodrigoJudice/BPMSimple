using BPMSimples.Motor.Alcadas;

namespace BPMSimples.Motor.Interfaces;


// Interface do motor de alçada
public interface IMotorAlcada
{
    StatusAlcada ObterStatus(decimal valor, List<RegistroAlcada> aprovacoesJaRealizadas, ISegurancaBPM seguranca);
    RequisitoAlcada? ObterRegra(decimal valor);
}
