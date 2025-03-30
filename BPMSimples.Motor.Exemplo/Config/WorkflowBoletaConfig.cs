using BPMSimples.Motor.Dominio;

namespace BPMSimples.Motor.Boleta.Config;

public static class WorkflowBoletaConfig
{
    public static class Estados
    {
        public static readonly EstadoBPM Estado1 = new("Estado1");
        public static readonly EstadoBPM Estado2 = new("Estado2");
        public static readonly EstadoBPM Estado3 = new("Estado3");
        public static readonly EstadoBPM Estado4 = new("Estado4");
        public static readonly EstadoBPM Estado5 = new("Estado5");
    }

    public static class Eventos
    {
        public static readonly EventoBPM Aprovar = new("Aprovar");
        public static readonly EventoBPM Enviar = new("Enviar");
        public static readonly EventoBPM Finalizar = new("Finalizar");
        public static readonly EventoBPM Rejeitar = new("Rejeitar");
        public static readonly EventoBPM Reprovar = new("Reprovar");
        public static readonly EventoBPM Salvar = new("Salvar");

    }

}

