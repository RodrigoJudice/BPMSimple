using BPMSimples.Motor.Alcadas;
using BPMSimples.Motor.Interfaces;

namespace BPMSimples.Motor.Boleta;

// Implementação do motor de alçada com base em transações
public class MotorAlcadaTransacional : IMotorAlcada
{
    private readonly List<RequisitoAlcada> _regras =
    [
        new() { ValorMin = 0, ValorMax = 100_000, Usuarios = ["joao"] },
        new() { ValorMin = 100_001, ValorMax = 500_000, Usuarios = ["joao","maria"] },
        new() { ValorMin = 500_001, ValorMax = decimal.MaxValue, Usuarios = ["vp"] },
    ];

    public StatusAlcada ObterStatus(decimal valor, List<RegistroAlcada> aprovacoesJaRealizadas, ISegurancaBPM seguranca)
    {
        var status = new StatusAlcada();

        var regra = _regras.FirstOrDefault(r => valor >= r.ValorMin && valor <= r.ValorMax);
        if (regra == null)
        {
            status.Faltantes = [];
            return status;
        }

        foreach (var usuarioEsperado in regra.Usuarios)
        {
            if (usuarioEsperado == seguranca.UserName)
                status.JaExecutadas.Add(usuarioEsperado);

            else
                status.Faltantes.Add(usuarioEsperado);
        }

        foreach (var usuario in aprovacoesJaRealizadas)
            status.JaExecutadas.Add(usuario.Usuario);

        return status;
    }

    public RequisitoAlcada? ObterRegra(decimal valor)
    {
        return _regras.FirstOrDefault(r => valor >= r.ValorMin && valor <= r.ValorMax);
    }


}

