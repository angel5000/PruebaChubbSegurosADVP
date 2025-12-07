using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.DTOS
{
    public class SegurosResponseDTO
    {
        public int IDSEGURO { get; set; }
        public string NMBRSEGURO { get; set; } = null!;
        public string CODSEGURO { get; set; } = null!;
        public decimal SUMASEGURADA { get; set; }
        public decimal PRIMA { get; set; }
        public int EDADMIN { get; set; }
        public int EDADMAX { get; set; }
        public string RangoEdad => $"{EDADMIN} - {EDADMAX} años";
        public string? USRCreacion { get; set; } = null!;
        public string? FechaCreacion { get; set; } = null!;
        public string? USRActualizacion { get; set; } = null!;
        public DateTime? FechaActualizacion { get; set; }
        public string? UsuarioIP { get; set; } = null!;
        public bool? Estado { get; set; }
    }
    public class SegurosResponseIDDTO
    {
        public int IDSEGURO { get; set; }
        public string NMBRSEGURO { get; set; } = null!;
        public string CODSEGURO { get; set; } = null!;
        public decimal SUMASEGURADA { get; set; }
        public decimal PRIMA { get; set; }
        public int EDADMIN { get; set; }
        public int EDADMAX { get; set; }
    }
    public class AseguradosporSeguraresponseDTO
    {
        public int IDASEGURADOS { get; set; }
        public string CEDULA { get; set; } = null!;
        public string NMBRCOMPLETO { get; set; } = null!;
        public int EDAD { get; set; }
        public string NMBRSEGURO { get; set; } = null!;
        public string? TELEFONO { get; set; } = null!;
        public string FECHACONTRATASEGURO { get; set; }
    }
}
