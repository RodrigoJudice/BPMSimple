using BPMSimples.Motor.Dominio;
using BPMSimples.Motor.Interfaces;
using BPMSimples.Motor.MaquinaEstado;

namespace BPMSimples.Motor.Boleta;

public class BoletaBancaria
{
    public Guid Id { get; set; }
    public string NumeroBoleta { get; set; } = default!;
    public string TipoBoleta { get; set; } = default!; // Ex: "Renda Fixa", "CDB", "Crédito", etc.

    // Cliente
    public Guid ClienteId { get; set; }
    public string NomeCliente { get; set; } = default!;
    public string CpfCnpj { get; set; } = default!;
    public string TipoPessoa { get; set; } = default!; // "Física" ou "Jurídica"

    // Dados financeiros
    public decimal ValorOperacao { get; set; }
    public string Moeda { get; set; } = "BRL";
    public DateTime DataOperacao { get; set; }
    public DateTime? DataLiquidacao { get; set; }
    public string Produto { get; set; } = default!; // Ex: "CDB", "LCI", "Financiamento", etc.
    public string InstituicaoFinanceira { get; set; } = default!;
    public int PrazoDias { get; set; }
    public decimal TaxaJuros { get; set; }
    public string TipoTaxa { get; set; } = default!; // Ex: "Pré", "Pós", "CDI", "IPCA"

    // Dados bancários
    public string BancoDestino { get; set; } = default!;
    public string AgenciaDestino { get; set; } = default!;
    public string ContaDestino { get; set; } = default!;
    public string TipoConta { get; set; } = default!; // "Corrente", "Poupança", etc.

    // Status e workflow
    public string StatusAtual { get; set; } = "EmPreenchimento";
    public Guid? IdWorkflowInstance { get; set; }
    public int VersaoBoleta { get; set; }

    // Justificativas e comentários
    public string? Observacoes { get; set; }
    public string? JustificativaAprovacao { get; set; }
    public string? JustificativaRejeicao { get; set; }

    // Auditoria
    public string CriadoPor { get; set; } = default!;
    public DateTime CriadoEm { get; set; }
    public string? AlteradoPor { get; set; }
    public DateTime? AlteradoEm { get; set; }
}
