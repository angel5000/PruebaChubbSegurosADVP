using Chubbseg.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Application.Interfaces
{
    public interface IAseguramientosApplication
    {
        Task<BaseResponse<IEnumerable<AseguramientoResponseDTO>>> ListaAseguramientos();
        Task<BaseResponse<bool>> RegistrarAseguramiento(AseguramientoRequestDTO request);
        Task<BaseResponse<bool>> EliminarAseguramiento(int AseguramientoID, string usuario);
    }
}
