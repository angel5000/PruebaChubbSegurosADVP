using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.DTOS
{
    public class RolesPermisosResponseDTO
    {
        public int? IdRol { get; set; }
        public int? IdPermiso { get; set; } = null!;
        public bool? Estado { get; set; } = null!;
        public DateTime FechaAsignacion { get; set; }

    }
}
