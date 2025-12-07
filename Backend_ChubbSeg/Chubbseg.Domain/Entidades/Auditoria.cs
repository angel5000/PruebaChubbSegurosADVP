using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Domain.Entidades
{
    public abstract class Auditoria
    {

         public DateTime? FechaCreacion { get; set; }

         public string? USRCreacion { get; set; } = null!;

         public DateTime? FechaActualizacion { get; set; }

         public string? USRActualizacion { get; set; } = null!;

         public string? UsuarioIP { get; set; } = null!;

         public bool? Estado { get; set; }
        public string? EstadoDT { get; set; }
    }
}
