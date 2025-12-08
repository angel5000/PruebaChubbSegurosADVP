using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.DTOS
{
    public class CobranzasResponseDTO
    {
        public int? IDCOBRANZA { get; set; }
        public int? IDASEGURADO { get; set; }
        public string? CLIENTE { get; set; } = null!;
        public string? POLIZA { get; set; } = null!;
        public DateOnly? FECHA_VENCIMIENTO { get; set; } = null!;
        public decimal? MONTO_ESPERADO { get; set; }
        public string? ESTADO_COBRANZA { get; set; } = null!;
        public string? ESTADO_CALCULADO { get; set; }
        public string? DIAS_RETRASO { get; set; } = null!;
    }
}
