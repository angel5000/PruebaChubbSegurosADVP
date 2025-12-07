using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Domain.Entidades
{
    public class Asegurados:Auditoria
    {
        public int IDASEGURADOS { get; set; }
        public string CEDULA { get; set; } = null!;
        public string NMBRCOMPLETO { get; set; } = null!;
        public string TELEFONO { get; set; } = null!;
        public int EDAD { get; set; }
    }
}
