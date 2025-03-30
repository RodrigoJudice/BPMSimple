using BPMSimples.Motor.Alcadas;
using BPMSimples.Motor.Interfaces;

namespace BPMSimples.Motor.Boleta;

// Implementação do motor de alçada com base em transações
public class MotorAlcadaTransacional : IMotorAlcada
{
    private readonly List<RequisitoAlcada> _regras =
    [
        new() { ValorMin = 0, ValorMax = 100_000, GruposDeUsuarios  = [["joao"]] },
        new() { ValorMin = 100_001, ValorMax = 500_000, GruposDeUsuarios  = [["joao","maria"], ["vp1"]] },
        new() { ValorMin = 500_001, ValorMax = decimal.MaxValue, GruposDeUsuarios  = [["vp2"]] },
    ];

    public StatusAlcada ObterStatus(decimal valor, List<RegistroAlcada> aprovacoesJaRealizadas, ISegurancaBPM seguranca)
    {
        var status = new StatusAlcada();

        var regra = ObterRegra(valor);
        if (regra == null)
            return status;

        var usuariosAprovados = aprovacoesJaRealizadas.Select(a => a.Usuario).ToHashSet();
        if (!usuariosAprovados.Contains(seguranca.UserName))
            usuariosAprovados.Add(seguranca.UserName); // conta o usuário atual também, se ainda não aprovado

        // Verifica se algum grupo está completamente aprovado
        var grupoAprovado = regra.GruposDeUsuarios.FirstOrDefault(grupo =>
            grupo.All(usuario => usuariosAprovados.Contains(usuario))
        );

        // Alternativa sem LINQ
        //List<string>? grupoAprovado = null;

        //foreach (var grupo in regra.GruposDeUsuarios)
        //{
        //    bool todosAprovaram = true;

        //    foreach (var usuario in grupo)
        //    {
        //        if (!usuariosAprovados.Contains(usuario))
        //        {
        //            todosAprovaram = false;
        //            break; // não precisa verificar o resto, já falhou
        //        }
        //    }

        //    if (todosAprovaram)
        //    {
        //        grupoAprovado = grupo;
        //        break; // já achou o primeiro grupo aprovado
        //    }
        //}


        if (grupoAprovado != null)
        {
            status.JaExecutadas = grupoAprovado;
            return status;
        }

        // Caso nenhum grupo esteja completo, mostra o grupo mais próximo (ex: o que o usuário faz parte)
        foreach (var grupo in regra.GruposDeUsuarios)
        {
            foreach (var usuario in grupo)
            {
                if (usuariosAprovados.Contains(usuario))
                    status.JaExecutadas.Add(usuario);
                else
                    status.Faltantes.Add(usuario);
            }

            // apenas mostra o primeiro grupo parcial
            break;
        }

        return status;
    }


    public RequisitoAlcada? ObterRegra(decimal valor)
    {
        return _regras.FirstOrDefault(r => valor >= r.ValorMin && valor <= r.ValorMax);
    }

    public List<List<string>> ObterGruposDeAprovadores(decimal valor)
    {
        var regra = ObterRegra(valor);

        // Retorna os grupos originais ou uma lista vazia se não houver regra
        return regra?.GruposDeUsuarios ?? [];
    }

    public string FormatarGruposComoTexto(decimal valor)
    {
        var grupos = ObterGruposDeAprovadores(valor);

        return string.Join(" OR ",
            grupos.Select(grupo =>
                "(" + string.Join(" AND ", grupo) + ")"
            )
        );
    }

    public string ObterGruposFormatadosParaDebug(decimal valor)
    {
        var grupos = ObterGruposDeAprovadores(valor);

        return string.Join(" | ", grupos.Select(g => $"[{string.Join(", ", g)}]"));
    }

}

