using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.DTOS
{
    public class AseguradosResponseDTO
    {
        public int IDASEGURADOS { get; set; }
        public string CEDULA { get; set; } = null!;
        public string NMBRCOMPLETO { get; set; } = null!;
        public string TELEFONO { get; set; } = null!;
        public int EDAD { get; set; }

    }
}
