using Chubbseg.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.Interfaces
{
    public interface ICobranzasApplication
    {
        Task<BaseResponse<IEnumerable<CobranzasResponseDTO>>> ListaCobranzas();
        Task<BaseResponse<bool>> CancelarSeguro(int AseguradoID, string usuariomod );

    }
}
