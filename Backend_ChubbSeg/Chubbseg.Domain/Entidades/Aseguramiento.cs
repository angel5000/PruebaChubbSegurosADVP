using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Domain.Entidades
{
    public class Aseguramiento : Auditoria
    {
        public int? IDASEGURADOS { get; set; }
        public int? IDUSRSEGUROS { get; set; }
        public int? IDSEGURO { get; set; }
        public string? CEDULA { get; set; } = null!;
        public string? NMBRCOMPLETO { get; set; } = null!;
        public string? TELEFONO { get; set; } = null!;
        public int? EDAD { get; set; }
        public string? NMBRSEGURO { get; set; } = null!;
        public string? CODSEGURO { get; set; } = null!;
        public decimal? SUMASEGURADA { get; set; }
        public decimal? PRIMA { get; set; }
        public string? FECHACONTRATASEGURO { get; set; }
    }
}
