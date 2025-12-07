using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Domain.Entidades
{
    public class Login
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public int IdRol { get; set; } 
        public string NombreRol{ get; set; } = null!;
        public int Estado { get; set; }
        public int Resultado { get; set; }

    }
}
