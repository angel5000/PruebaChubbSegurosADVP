using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.DTOS
{
    public class AseguramientoResponseDTO
    {
        public int IDASEGURADOS { get; set; }
        public int IDUSRSEGUROS { get; set; }
        public string CEDULA { get; set; } = null!;
        public string NMBRCOMPLETO { get; set; } = null!;
        public int EDAD { get; set; }
        public string NMBRSEGURO { get; set; } = null!;
        public string CODSEGURO { get; set; } = null!;
        public decimal SUMASEGURADA { get; set; }
        public decimal PRIMA { get; set; }
        public string FECHACONTRATASEGURO { get; set; }
        public string? USRCreacion { get; set; } = null!;
        public string? FechaCreacion { get; set; } = null!;
        public string? USRActualizacion { get; set; } = null!;
        public DateTime? FechaActualizacion { get; set; }
        public string? UsuarioIP { get; set; } = null!;
        public bool? Estado { get; set; }
    }
}
