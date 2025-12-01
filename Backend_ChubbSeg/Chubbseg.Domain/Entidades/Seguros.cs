using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Domain.Entidades
{
    public class Seguros
    {
        public int IDSEGURO { get; set; }
        public string NMBRSEGURO { get; set; }=null!;
        public string CODSEGURO { get; set; }=null!;
        public decimal SUMASEGURADA { get; set; }
        public decimal PRIMA { get; set; }
        public int EDADMIN { get; set; }
        public int EDADMAX { get; set; }

    }
}
