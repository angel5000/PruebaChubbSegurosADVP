

namespace Chubbseg.Domain.Entidades
{
    public class Cobranzas:Auditoria
    {
        public int? IDCOBRANZA { get; set; } 
        public int? IDUSRSEGUROSFK { get; set; } 
        public DateOnly? FECHA_EMISION { get; set; } = null!;
        public DateOnly? FECHA_VENCIMIENTO { get; set; } = null!;
        public decimal? MONTO_ESPERADO { get; set; } 
        public decimal? MONTO_PAGADO { get; set; } 
        public DateTime? FECHA_PAGO { get; set; } = null!;
        public string? METODO_PAGO { get; set; }
        public string? REFERENCIA_PAGO { get; set; } = null!;
        public string? OBSERVACION { get; set; } = null!;
        public string? ESTADO_COBRANZA { get; set; } = null!;

    }
}
