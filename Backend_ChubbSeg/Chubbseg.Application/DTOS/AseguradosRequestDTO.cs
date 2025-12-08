using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.DTOS
{
    public class AseguradosRequestDTO
    {
        public string CEDULA { get; set; } = null!;
        public string NMBRCOMPLETO { get; set; } = null!;
        public string TELEFONO { get; set; } = null!;
        public int EDAD { get; set; }
        public string? CODSEGURO { get; set; }=null;

    }
    public class AseguradosEditRequestDTO
    {
        public string CEDULA { get; set; } = null!;
        public string NMBRCOMPLETO { get; set; } = null!;
        public string TELEFONO { get; set; } = null!;
        public int EDAD { get; set; }
        public string? USRActualizacion { get; set; } = null!;
        public int? Estado { get; set; }

    }
}
