using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.DTOS
{
   public class SegurosRequesteditDTO
    {
        public string NMBRSEGURO { get; set; } = null!;
        public string CODSEGURO { get; set; } = null!;
         public decimal SUMASEGURADA { get; set; }
         public decimal PRIMA { get; set; }
         public int EDADMIN { get; set; }
         public int EDADMAX { get; set; }
         public string? USRActualizacion { get; set; } = null!;
        public string? UsuarioIP { get; set; } = null!;
        public int? Estado { get; set; }
    }
    
    public class SegurosRequestDTO
    {
        public string NMBRSEGURO { get; set; }
        public string CODSEGURO { get; set; }
        public decimal SUMASEGURADA { get; set; }
        public decimal PRIMA { get; set; }
        public int EDADMIN { get; set; }
        public int EDADMAX { get; set; }
        public string USRCreacion { get; set; }
        public string? UsuarioIP { get; set; } = null!;
        public int Estado { get; set; }
    }

    public class SegurosEditRequestDTO
    {
        public string NMBRSEGURO { get; set; }
        public string CODSEGURO { get; set; }
        public decimal SUMASEGURADA { get; set; }
        public decimal PRIMA { get; set; }
        public int EDADMIN { get; set; }
        public int EDADMAX { get; set; }
        public string? USRActualizacion { get; set; } = null!;
        public DateTime? FechaActualizacion { get; set; }
        public string? UsuarioIP { get; set; } = null!;
        public int? Estado { get; set; }
    }

    public class SegurosRequestDeleteDTO
    {
   
        public string? USRActualizacion { get; set; } = null!;
        public string? UsuarioIP { get; set; } = null!;
        public string? EstadoDT { get; set; }
    }
}
